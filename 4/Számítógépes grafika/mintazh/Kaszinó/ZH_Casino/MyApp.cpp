#include "MyApp.h"
#include "ObjParser.h"
#include "SDL_GLDebugMessageCallback.h"

#include <imgui.h>

#include "ZHUtils.h"

CMyApp::CMyApp() : m_cameraManipulator(m_camera)
{
}

CMyApp::~CMyApp()
{
}

void CMyApp::SetupDebugCallback()
{
	// engedélyezzük és állítsuk be a debug callback függvényt ha debug context-ben vagyunk
	GLint context_flags;
	glGetIntegerv(GL_CONTEXT_FLAGS, &context_flags);
	if (context_flags & GL_CONTEXT_FLAG_DEBUG_BIT) {
		glEnable(GL_DEBUG_OUTPUT);
		glEnable(GL_DEBUG_OUTPUT_SYNCHRONOUS);
		glDebugMessageControl(GL_DONT_CARE, GL_DONT_CARE, GL_DEBUG_SEVERITY_NOTIFICATION, 0, nullptr, GL_FALSE);
		glDebugMessageControl(GL_DONT_CARE, GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR, GL_DONT_CARE, 0, nullptr, GL_FALSE);
		glDebugMessageCallback(SDL_GLDebugMessageCallback, nullptr);
	}
}

void CMyApp::InitShaders()
{
	m_programID = glCreateProgram();
	AttachShader(m_programID, GL_VERTEX_SHADER, "Shaders/Vert_PosNormTex.vert");
	AttachShader(m_programID, GL_FRAGMENT_SHADER, "Shaders/Frag_ZH.frag");
	LinkProgram(m_programID);
}

void CMyApp::CleanShaders()
{
	glDeleteProgram(m_programID);
}

void CMyApp::InitGeometry()
{
	const std::initializer_list<VertexAttributeDescriptor> vertexAttribList =
	{
		{0, offsetof(Vertex, position), 3, GL_FLOAT},
		{1, offsetof(Vertex, normal), 3, GL_FLOAT},
		{2, offsetof(Vertex, texcoord), 2, GL_FLOAT},
	};

	m_quadGPU = CreateGLObjectFromMesh(createQuad(), vertexAttribList);
	m_handGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/hand.obj"), vertexAttribList);
	m_chipGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/pokerchip.obj"), vertexAttribList);
}

void CMyApp::CleanGeometry()
{
	CleanOGLObject(m_quadGPU);
	CleanOGLObject(m_handGPU);
	CleanOGLObject(m_chipGPU);
}

void CMyApp::InitTextures()
{
	glCreateSamplers(1, &m_SamplerID);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	ImageRGBA imageTable = ImageFromFile("Assets/green_fabric.jpg");

	glCreateTextures(GL_TEXTURE_2D, 1, &m_tableTextureID);
	glTextureStorage2D(m_tableTextureID, NumberOfMIPLevels(imageTable), GL_RGBA8, imageTable.width, imageTable.height);
	glTextureSubImage2D(m_tableTextureID, 0, 0, 0, imageTable.width, imageTable.height, GL_RGBA, GL_UNSIGNED_BYTE, imageTable.data());
	glGenerateTextureMipmap(m_tableTextureID);

	// 2. Kártya textúra betöltése
	ImageRGBA imageCard = ImageFromFile("Assets/cards.png");
	glCreateTextures(GL_TEXTURE_2D, 1, &m_cardTextureID);
	glTextureStorage2D(m_cardTextureID, NumberOfMIPLevels(imageCard), GL_RGBA8, imageCard.width, imageCard.height);
	glTextureSubImage2D(m_cardTextureID, 0, 0, 0, imageCard.width, imageCard.height, GL_RGBA, GL_UNSIGNED_BYTE, imageCard.data());
	glGenerateTextureMipmap(m_cardTextureID);

	// 3. Kézfej textúra betöltése
	ImageRGBA imageHand = ImageFromFile("Assets/hand.png");
	glCreateTextures(GL_TEXTURE_2D, 1, &m_handTextureID);
	glTextureStorage2D(m_handTextureID, NumberOfMIPLevels(imageHand), GL_RGBA8, imageHand.width, imageHand.height);
	glTextureSubImage2D(m_handTextureID, 0, 0, 0, imageHand.width, imageHand.height, GL_RGBA, GL_UNSIGNED_BYTE, imageHand.data());
	glGenerateTextureMipmap(m_handTextureID);

	ImageRGBA imageChip = ImageFromFile("Assets/pokerchip.jpg");
	glCreateTextures(GL_TEXTURE_2D, 1, &m_chipTextureID);
	glTextureStorage2D(m_chipTextureID, NumberOfMIPLevels(imageChip), GL_RGBA8, imageChip.width, imageChip.height);
	glTextureSubImage2D(m_chipTextureID, 0, 0, 0, imageChip.width, imageChip.height, GL_RGBA, GL_UNSIGNED_BYTE, imageChip.data());
	glGenerateTextureMipmap(m_chipTextureID);
}

void CMyApp::CleanTextures()
{
	glDeleteTextures(1, &m_tableTextureID);
	glDeleteTextures(1, &m_cardTextureID);
	glDeleteTextures(1, &m_handTextureID);
	glDeleteTextures(1, &m_chipTextureID);
	glDeleteSamplers(1, &m_SamplerID);
}

bool CMyApp::Init()
{
	SetupDebugCallback();

	// törlési szín legyen kékes
	glClearColor(0.125f, 0.25f, 0.5f, 1.0f);

	InitShaders();
	InitGeometry();
	InitTextures();

	m_cards.clear();
	for (int i = 0; i < 5; ++i) {
		CardState card;
		card.position = glm::vec3(-4.0f + i * 2.0f, 0.0f, 0.0f);
		card.type = i; // <--- Itt állítjuk be, hogy mind az 5 lap más legyen! (0, 1, 2, 3, 4)
		m_cards.push_back(card);
	}

	//
	// egyéb inicializálás
	//

	glEnable(GL_CULL_FACE); // kapcsoljuk be a hátrafelé néző lapok eldobását 
	glCullFace(GL_BACK);    // GL_BACK: a kamerától "elfelé" néző lapok, GL_FRONT: a kamera felé néző lapok 

	glEnable(GL_DEPTH_TEST); // mélységi teszt bekapcsolása (takarás) 

	// kamera 
	m_camera.SetView(
		glm::vec3(0.0, 30.0, 25.0),  // honnan nézzük a színteret - eye
		glm::vec3(0.0, 0.0, 0.0),  // a színtér melyik pontját nézzük - at
		glm::vec3(0.0, 1.0, 0.0)); // felfelé mutató irány a világban - up

	m_cameraManipulator.SetStateFromCamera();

	return true;
}

void CMyApp::Clean()
{
	CleanShaders();
	CleanGeometry();
	CleanTextures();
}

Ray CMyApp::CalculatePixelRay(glm::vec2 pixel) const
{
	// NDC koordináták kiszámítása 
	glm::vec4 pickedNDC = glm::vec4(
		2.0f * (pixel.x + 0.5f) / m_windowSize.x - 1.0f,
		1.0f - 2.0f * (pixel.y + 0.5f) / m_windowSize.y, 0.0f,1.0);

	// A világ koordináták kiszámítása az inverz ViewProj mátrix segítségével
	glm::vec4 pickedWorld = glm::inverse(m_camera.GetViewProj()) * pickedNDC;
	pickedWorld /= pickedWorld.w; // homogén osztás 

	Ray ray;

	// Raycasting kezdőpontja a kamera pozíciója 
	ray.origin = m_camera.GetEye();
	// Iránya a kamera pozíciójából a kattintott pont világ koordinátái felé 
	// FIGYELEM: NEM egység hosszúságú vektor! 
	ray.direction = glm::vec3(pickedWorld) - ray.origin;
	return ray;
}

void CMyApp::Update(const SUpdateInfo& updateInfo)
{
	m_ElapsedTimeInSec = updateInfo.ElapsedTimeInSec;

	//if (m_IsPicking) {
	//	// a felhasználó Ctrl + kattintott, itt kezeljük le
	//	// sugár indítása a kattintott pixelen át
	//	Ray ray = CalculatePixelRay(glm::vec2(m_PickedPixel.x, m_PickedPixel.y));
	//	
 //       m_IsPicking = false;
	//}
	//
	//m_cameraManipulator.Update(updateInfo.DeltaTimeInSec);

    m_objectWorldTransform = glm::translate( OBJECT_POS );

	// --- Fény mozgatása ---
	if (m_lightMoving) {
		// Csak akkor növeljük az időt, ha a fény mozog
		m_lightTime += updateInfo.DeltaTimeInSec;
	}

	// Elágazások helyett tiszta matematikai koszinusz alapú hullám (4 másodperces periódussal)
	float t = 0.5f - 0.5f * cos((m_lightTime / 4.0f) * 2.0f * glm::pi<float>());

	// Pozíció kiszámítása a feladatban szereplő eredeti képlet alapján (ez nem változik)
	float x = -15.0f + 30.0f * t;
	float y = 10.0f - 20.0f * t + 20.0f * t * t;
	float z = 0.0f;

	// Beállítjuk a kiszámolt pontot
	m_lightPosition = glm::vec4(x, y, z, 1.0f);
}

void CMyApp::SetCommonUniforms()
{
	// - Uniform paraméterek 

	// - view és projekciós mátrix 
	glUniformMatrix4fv( ul("viewProj"), 1, GL_FALSE, glm::value_ptr(m_camera.GetViewProj()));

	// - Fényforrások beállítása 
	glUniform3fv( ul("cameraPosition"), 1, glm::value_ptr(m_camera.GetEye()));
	glUniform4fv( ul("lightPosition"), 1, glm::value_ptr(m_lightPosition));
	
	glUniform3fv( ul("La"), 1, glm::value_ptr(m_La));
	glUniform3fv( ul("Ld"), 1, glm::value_ptr(m_Ld));
	glUniform3fv( ul("Ls"), 1, glm::value_ptr(m_Ls));
	
	glUniform1f( ul("lightConstantAttenuation"), m_lightConstantAttenuation);
	glUniform1f( ul("lightLinearAttenuation"), m_lightLinearAttenuation);
	glUniform1f( ul("lightQuadraticAttenuation"), m_lightQuadraticAttenuation);

	glUniform1i(ul("shadingEnabled"), m_shadingEnabled ? 1 : 0);
}

void CMyApp::DrawObject(OGLObject& obj, const glm::mat4& world) {
	glUniformMatrix4fv(ul("world"), 1, GL_FALSE, glm::value_ptr(world));
	glUniformMatrix4fv(ul("worldInvTransp"), 1, GL_FALSE, glm::value_ptr(glm::transpose(glm::inverse(world))));
	glBindVertexArray(obj.vaoID);
	glDrawElements(GL_TRIANGLES, obj.count, GL_UNSIGNED_INT, nullptr);
}

void CMyApp::RenderTable() {
	glBindTextureUnit(0, m_tableTextureID);
	glm::mat4 world = glm::translate(glm::vec3(0.0f, -0.1f, 0.0f)) * glm::scale(glm::vec3(30.0f, 0.0f, 21.0f));
	DrawObject(m_quadGPU, world);
}

void CMyApp::RenderCards() {
	glBindTextureUnit(0, m_cardTextureID);
	glUniform1i(ul("objectType"), SHADER_STATE_CARD);
	for (const CardState& card : m_cards) {
		glUniform1i(ul("cardType"), card.type);

		glm::mat4 world = glm::translate(card.position) * glm::scale(glm::vec3(1.6f, 1.0f, 2.48f));
		DrawObject(m_quadGPU, world);
	}

	// A rajzolás végén visszaállítjuk az állapotot alapértelmezettre!
	glUniform1i(ul("objectType"), SHADER_STATE_DEFAULT);
}

void CMyApp::RenderHand()
{
	glBindTextureUnit(0, m_handTextureID);
	// Kézfej pozíciója, Y tengely körüli forgatása és skálázása (3x)
	glm::mat4 world = glm::translate(glm::vec3(-3.0f, 2.0f, 12.0f)) *
		glm::rotate(m_handRotation, glm::vec3(0.0f, 1.0f, 0.0f)) *
		glm::scale(glm::vec3(3.0f, 3.0f, 3.0f));
	DrawObject(m_handGPU, world);
}

void CMyApp::RenderChip()
{
	glBindTextureUnit(0, m_chipTextureID);

	// Megmondjuk a shadernek, hogy most zsetonokat rajzolunk
	glUniform1i(ul("objectType"), SHADER_STATE_CHIP);

	// A torony középpontja a feladat alapján
	glm::vec3 towerCenter(-10.0f, 0.0f, 5.0f);

	// --- 1. Torony rajzolása (N db zseton) ---
	for (int i = 0; i < m_chipCountN; ++i) {
		// Elküldjük a zseton sorszámát a szín kiválasztásához
		glUniform1i(ul("colorIndex"), i);

		// A zsetonok magassága 0.15 egység, így minden zsetont ennyivel tolunk feljebb az előzőnél
		glm::mat4 world = glm::translate(towerCenter + glm::vec3(0.0f, i * 0.15f, 0.0f));
		DrawObject(m_chipGPU, world);
	}

	// --- 2. Guruló zsetonok rajzolása (K db zseton) ---
	float radius = 4.0f; // Keringési sugár
	float time = m_ElapsedTimeInSec; // Az idő múlása felel a mozgásért

	for (int j = 0; j < m_chipCountK; ++j) {
		// Itt is elküldjük a sorszámot
		glUniform1i(ul("colorIndex"), j);

		// Kiszámoljuk, hogy az adott zseton éppen hol tart a kör mentén.
		// A kör 360 fokát (2 PI) elosztjuk a zsetonok számával, így egyenletesen oszlanak el.
		float orbitAngle = (j / (float)m_chipCountK) * 2.0f * glm::pi<float>() + time;

		// A gurulás szöge. Mivel a kör sugara 4, a zseton sugara pedig 1, 
		// egy teljes keringés alatt a zseton 4-szer fordul körbe a saját tengelye körül.
		float rollAngle = orbitAngle * radius;

		// A mátrixokat alulról felfelé érdemes olvasni az értelmezéshez!
		glm::mat4 world =
			glm::translate(towerCenter) * // 5. Végül áttoljuk az egész kört a torony köré
			glm::rotate(orbitAngle, glm::vec3(0.0f, 1.0f, 0.0f)) * // 4. Keringés a középpont körül
			glm::translate(glm::vec3(radius, 1.0f, 0.0f)) * // 3. Eltolás 4 egységre (keringési sugár), és felemelés 1 egységgel (hogy az asztalon álljon, ne benne)
			glm::rotate(glm::half_pi<float>(), glm::vec3(0.0f, 0.0f, 1.0f)) * // 2. Felállítás az élére (Z tengely körül 90 fokos forgatás)
			glm::rotate(rollAngle, glm::vec3(0.0f, 1.0f, 0.0f));               // 1. Saját Y tengelye körüli pörgés (ez adja a gurulás hatását)

		DrawObject(m_chipGPU, world);
	}

	// A rajzolás végén visszaállítjuk az állapotot alapértelmezettre, 
	// hogy a következő képkockánál az asztal és a kártyák ne színezződjenek el!
	glUniform1i(ul("objectType"), SHADER_STATE_DEFAULT);
}

void CMyApp::Render()
{
	// töröljük a framepuffert (GL_COLOR_BUFFER_BIT)...
	// ... és a mélységi Z puffert (GL_DEPTH_BUFFER_BIT)
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glUseProgram(m_programID);

	SetCommonUniforms();


	glUniform1i(ul("textureImage"), 0);
	glBindSampler(0, m_SamplerID);

	RenderTable();
	RenderCards();
	RenderHand();
	RenderChip();

	glBindTextureUnit(0, 0);
	glBindSampler(0, 0);

	glBindVertexArray(0);
	// shader kikapcsolása 
	glUseProgram(0);
}

void CMyApp::RenderGUI()
{
	ImGui::Begin("Kaszinó Vezérlés");
	ImGui::SliderAngle("Kéz forgatása", &m_handRotation, -45.0f, 45.0f);

	// Új csúszkák a zsetonok számának szabályozásához (1 és 20 között)
	ImGui::SeparatorText("Zsetonok beállítása");
	ImGui::SliderInt("Torony zsetonok (N)", &m_chipCountN, 1, 20);
	ImGui::SliderInt("Keringo zsetonok (K)", &m_chipCountK, 1, 20);

	ImGui::SeparatorText("Világítás beállítása");
	ImGui::Checkbox("Fény mozgatása", &m_lightMoving);
	ImGui::Checkbox("Árnyalás bekapcsolva", &m_shadingEnabled);

	ImGui::End();
}

// https://wiki.libsdl.org/SDL3/SDL_KeyboardEvent
// https://wiki.libsdl.org/SDL3/SDL_Keysym
// https://wiki.libsdl.org/SDL3/SDL_Keycode
// https://wiki.libsdl.org/SDL3/SDL_Keymod

void CMyApp::KeyboardDown(const SDL_KeyboardEvent& key)
{
	if (!key.repeat) // Először lett megnyomva 
	{
		if (key.key == SDLK_F5 && key.mod & SDL_KMOD_CTRL)
		{
			CleanShaders();
			InitShaders();
		}
		if (key.key == SDLK_F1)
		{
			GLint polygonModeFrontAndBack[2] = {};
			// https://registry.khronos.org/OpenGL-Refpages/gl4/html/glGet.xhtml
			glGetIntegerv(GL_POLYGON_MODE, polygonModeFrontAndBack); // Kérdezzük le a jelenlegi polygon módot! Külön adja a front és back módokat. 
			GLenum polygonMode = (polygonModeFrontAndBack[0] != GL_FILL ? GL_FILL : GL_LINE); // Váltogassuk FILL és LINE között! 
			// https://registry.khronos.org/OpenGL-Refpages/gl4/html/glPolygonMode.xhtml
			glPolygonMode(GL_FRONT_AND_BACK, polygonMode); // Állítsuk be az újat! 
		}

		if (key.key == SDLK_LCTRL || key.key == SDLK_RCTRL)
		{
			m_IsCtrlDown = true;
		}
	}
	m_cameraManipulator.KeyboardDown(key);
}

void CMyApp::KeyboardUp(const SDL_KeyboardEvent& key)
{
	m_cameraManipulator.KeyboardUp(key);
	if (key.key == SDLK_LCTRL || key.key == SDLK_RCTRL)
	{
		m_IsCtrlDown = false;
	}
}

// https://wiki.libsdl.org/SDL3/SDL_MouseMotionEvent

void CMyApp::MouseMove(const SDL_MouseMotionEvent& mouse)
{
	m_cameraManipulator.MouseMove(mouse);
}

// https://wiki.libsdl.org/SDL3/SDL_MouseButtonEvent

void CMyApp::MouseDown(const SDL_MouseButtonEvent& mouse)
{
	if (m_IsCtrlDown)
	{
		m_IsPicking = true;
	}
	m_PickedPixel = { mouse.x, mouse.y };
}

void CMyApp::MouseUp(const SDL_MouseButtonEvent& mouse)
{
}

// https://wiki.libsdl.org/SDL3/SDL_MouseWheelEvent

void CMyApp::MouseWheel(const SDL_MouseWheelEvent& wheel)
{
	m_cameraManipulator.MouseWheel(wheel);
}

// a két paraméterben az új ablakméret szélessége (_w) és magassága (_h) található
void CMyApp::Resize(int _w, int _h)
{
	glViewport(0, 0, _w, _h);
	m_windowSize = glm::uvec2(_w, _h);
	m_camera.SetAspect(static_cast<float>(_w) / _h);
}

// Le nem kezelt, egzotikus esemény kezelése
// https://wiki.libsdl.org/SDL3/SDL_Event

void CMyApp::OtherEvent(const SDL_Event& ev)
{

}