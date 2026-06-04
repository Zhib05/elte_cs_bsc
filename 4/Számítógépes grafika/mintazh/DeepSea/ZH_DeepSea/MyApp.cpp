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
	m_fishGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/PufferFish.obj"), vertexAttribList);
	m_subGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/sub.obj"), vertexAttribList);
	m_armGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/Arm.obj"), vertexAttribList);
	m_clawGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/Claw.obj"), vertexAttribList);
}

void CMyApp::CleanGeometry()
{
	CleanOGLObject(m_quadGPU);
	CleanOGLObject(m_fishGPU);
	CleanOGLObject(m_subGPU);
	CleanOGLObject(m_armGPU);
	CleanOGLObject(m_clawGPU);
}

void CMyApp::InitTextures()
{
	glCreateSamplers(1, &m_SamplerID);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_WRAP_S, GL_MIRRORED_REPEAT);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_WRAP_T, GL_MIRRORED_REPEAT);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	ImageRGBA talajImage = ImageFromFile("Assets/oceanBottom.png");
	glCreateTextures(GL_TEXTURE_2D, 1, &m_talajTextureID);
	glTextureStorage2D(m_talajTextureID, NumberOfMIPLevels(talajImage), GL_RGBA8, talajImage.width, talajImage.height);
	glTextureSubImage2D(m_talajTextureID, 0, 0, 0, talajImage.width, talajImage.height, GL_RGBA, GL_UNSIGNED_BYTE, talajImage.data());
	glGenerateTextureMipmap(m_talajTextureID);

	ImageRGBA oceanImage = ImageFromFile("Assets/ocean.png");
	glCreateTextures(GL_TEXTURE_2D, 1, &m_oceanTextureID);
	glTextureStorage2D(m_oceanTextureID, NumberOfMIPLevels(oceanImage), GL_RGBA8, oceanImage.width, oceanImage.height);
	glTextureSubImage2D(m_oceanTextureID, 0, 0, 0, oceanImage.width, oceanImage.height, GL_RGBA, GL_UNSIGNED_BYTE, oceanImage.data());
	glGenerateTextureMipmap(m_oceanTextureID);

	ImageRGBA fishImage = ImageFromFile("Assets/PufferFish.png");
	glCreateTextures(GL_TEXTURE_2D, 1, &m_fishTextureID);
	glTextureStorage2D(m_fishTextureID, NumberOfMIPLevels(fishImage), GL_RGBA8, fishImage.width, fishImage.height);
	glTextureSubImage2D(m_fishTextureID, 0, 0, 0, fishImage.width, fishImage.height, GL_RGBA, GL_UNSIGNED_BYTE, fishImage.data());
	glGenerateTextureMipmap(m_fishTextureID);

	ImageRGBA subImage = ImageFromFile("Assets/sub.png");
	glCreateTextures(GL_TEXTURE_2D, 1, &m_subTextureID);
	glTextureStorage2D(m_subTextureID, NumberOfMIPLevels(subImage), GL_RGBA8, subImage.width, subImage.height);
	glTextureSubImage2D(m_subTextureID, 0, 0, 0, subImage.width, subImage.height, GL_RGBA, GL_UNSIGNED_BYTE, subImage.data());
	glGenerateTextureMipmap(m_subTextureID);
}

void CMyApp::CleanTextures()
{
	glDeleteTextures(1, &m_talajTextureID);
	glDeleteTextures(1, &m_oceanTextureID);
	glDeleteTextures(1, &m_fishTextureID);
	glDeleteTextures(1, &m_subTextureID);

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

	m_fishes.clear();
	for (int i = 0; i < 5; i++) {
		FishState fish;
		fish.position = glm::vec3(100.0f * cos(2.0f * glm::pi<float>() * ((float)i / 5.0f)), -140.0f + 130.f * ((float)i / 5.0f), 100.0f * sin(2.0f * glm::pi<float>() * ((float)i / 5.0f)));
		fish.rot_Angle = 0.0f;
		m_fishes.push_back(fish);
	}

	//
	// egyéb inicializálás
	//

	glEnable(GL_CULL_FACE); // kapcsoljuk be a hátrafelé néző lapok eldobását 
	glCullFace(GL_BACK);    // GL_BACK: a kamerától "elfelé" néző lapok, GL_FRONT: a kamera felé néző lapok 

	glEnable(GL_DEPTH_TEST); // mélységi teszt bekapcsolása (takarás) 

	// kamera 
	m_camera.SetView(
		glm::vec3(0.0, -50.0, 105.0),  // honnan nézzük a színteret - eye
		glm::vec3(0.0, -55.0, 100.0),  // a színtér melyik pontját nézzük - at
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

	// 2.2-es feladat: Törlési szín beállítása a kamera mélysége alapján
	float camY = m_camera.GetEye().y;
	float depthY = glm::min(0.0f, camY); // Csak a negatív (víz alatti) értékeket vesszük figyelembe

	// A képlet kiszámítása vektorizált formában
	glm::vec3 extinction = glm::exp(glm::vec3(0.014f, 0.01f, 0.004f) * depthY);

	// Az új törlési szín beállítása (az alpha csatorna marad 1.0)
	glClearColor(extinction.r, extinction.g, extinction.b, 1.0f);

	if (m_IsPicking) {
		// a felhasználó Ctrl + kattintott, itt kezeljük le
		// sugár indítása a kattintott pixelen át
		Ray ray = CalculatePixelRay(glm::vec2(m_PickedPixel.x, m_PickedPixel.y));
		
        m_IsPicking = false;
	}

	m_cameraManipulator.Update(updateInfo.DeltaTimeInSec);

    m_objectWorldTransform = glm::translate( OBJECT_POS );

	subBaseT = glm::translate(glm::vec3(0.0f, -140.0f, 0.0f));
	armT = subBaseT * glm::translate(glm::vec3(18.75f, -3.75f, 0.0f)) * glm::rotate(m_armR, glm::vec3(0.0f, 1.0f, 0.0f));
	lClawT = armT * glm::translate(glm::vec3(9.0f, 0.0f, -1.75f)) * glm::rotate(m_clawR, glm::vec3(0.0f, 1.0f, 0.0f));
	rClawT = armT * glm::translate(glm::vec3(9.0f, 0.0f, 1.75f)) * glm::rotate(glm::pi<float>(), glm::vec3(1.0f, 0.0f, 0.0f)) * glm::rotate(m_clawR, glm::vec3(0.0f, 1.0f, 0.0f));
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

	glUniform1f(ul("ETimeInSec"), m_ElapsedTimeInSec);

	// 2.3-as feladat: Piros jelzőfény paramétereinek átadása
	glm::vec4 localLightPos = glm::vec4(-13.0f, 9.0f, 0.0f, 1.0f);
	glm::vec4 worldLightPos = subBaseT * localLightPos;

	glUniform4fv(ul("light2Position"), 1, glm::value_ptr(worldLightPos));
	glUniform3f(ul("light2Ld"), 1.0f, 0.0f, 0.0f); // Tiszta piros diffúz szín
	glUniform3f(ul("light2Ls"), 1.0f, 0.0f, 0.0f); // Tiszta piros spekuláris szín

	glUniform1f(ul("light2ConstantAttenuation"), 1.0f);
	glUniform1f(ul("light2LinearAttenuation"), 0.02f);   // Hatótávolság beállítása
	glUniform1f(ul("light2QuadraticAttenuation"), 0.002f);

	glUniform1i(ul("isLight2On"), m_isWarningLightOn ? 1 : 0);
}

void CMyApp::DrawObject(OGLObject& obj, const glm::mat4& world) {
	glUniformMatrix4fv(ul("world"), 1, GL_FALSE, glm::value_ptr(world));
	glUniformMatrix4fv(ul("worldInvTransp"), 1, GL_FALSE, glm::value_ptr(glm::transpose(glm::inverse(world))));
	glBindVertexArray(obj.vaoID);
	glDrawElements(GL_TRIANGLES, obj.count, GL_UNSIGNED_INT, nullptr);
}

void CMyApp::RenderTalaj() {
	glBindTextureUnit(0, m_talajTextureID);
	glUniform1i(ul("ObjectType"), SHADER_STATE_TALAJ);
	glUniform1i(ul("isWorldCoordsTex"), 1);
	glm::mat4 world = glm::translate(glm::vec3(0.0f, -150.0f, 0.0f)) * glm::rotate(glm::half_pi<float>(), glm::vec3(0.0f, 1.0f, 0.0f)) * glm::scale(glm::vec3(1000.0f, 1.0f, 1000.0f));
	DrawObject(m_quadGPU, world);
}

void CMyApp::RenderOcean() {
	glBindTextureUnit(0, m_oceanTextureID);
	glUniform1i(ul("ObjectType"), SHADER_STATE_OCEAN);
	glUniform1i(ul("isWorldCoordsTex"), 1);
	glm::mat4 world = glm::translate(glm::vec3(0.0f, 0.0f, 0.0f)) * glm::scale(glm::vec3(1000.0f, 1.0f, 1000.0f));
	DrawObject(m_quadGPU, world);
}

void CMyApp::RenderFish() {
	glBindTextureUnit(0, m_fishTextureID);
	glUniform1i(ul("isWorldCoordsTex"), 0);
	for (const FishState& fish : m_fishes) {
		glm::mat4 world = glm::translate(fish.position) * glm::rotate(fish.rot_Angle, glm::vec3(0.0f, 1.0f, 0.0f));
		DrawObject(m_fishGPU, world);
	}
}

void CMyApp::RenderSub() {
	glBindTextureUnit(0, m_subTextureID);
	glUniform1i(ul("isWorldCoordsTex"), 0);
	DrawObject(m_subGPU, subBaseT);
	DrawObject(m_armGPU, armT);
	DrawObject(m_clawGPU, lClawT);
	DrawObject(m_clawGPU, rClawT);
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

	RenderTalaj();
	RenderOcean();
	RenderFish();
	RenderSub();

	glBindTextureUnit(0, 0);
	glBindSampler(0, 0);

	glBindVertexArray(0);
	// shader kikapcsolása 
	glUseProgram(0);
}

void CMyApp::RenderGUI()
{
	ImGui::Begin("Tengeralatjaro");
	ImGui::SliderAngle("Arm", &m_armR, -90.0f, 90.0f);
	ImGui::SliderAngle("Claw", &m_clawR, 0.0f, 90.0f);

	ImGui::Checkbox("Jelzofeny", &m_isWarningLightOn);
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