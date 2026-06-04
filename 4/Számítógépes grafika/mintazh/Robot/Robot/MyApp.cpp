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

	m_cubeGPU = CreateGLObjectFromMesh(createCube(), vertexAttribList);

	m_headGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/head.obj"), vertexAttribList);
	m_torsoGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/torso.obj"), vertexAttribList);
	m_leftUpperArmGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/left_upper_arm.obj"), vertexAttribList);
	m_leftLowerArmGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/left_lower_arm.obj"), vertexAttribList);
	m_rightUpperArmGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/right_upper_arm.obj"), vertexAttribList);
	m_rightLowerArmGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/right_lower_arm.obj"), vertexAttribList);
	m_legGPU = CreateGLObjectFromMesh(ObjParser::parse("Assets/leg.obj"), vertexAttribList);
}

void CMyApp::CleanGeometry()
{
	CleanOGLObject(m_cubeGPU);
}

void CMyApp::InitTextures()
{
	glCreateSamplers(1, &m_SamplerID);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
	glSamplerParameteri(m_SamplerID, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	ImageRGBA woodImage = ImageFromFile("Assets/Wood_Table_Texture.png");

	glCreateTextures(GL_TEXTURE_2D, 1, &m_woodTextureID);
	glTextureStorage2D(m_woodTextureID, NumberOfMIPLevels(woodImage), GL_RGBA8, woodImage.width, woodImage.height);
	glTextureSubImage2D(m_woodTextureID, 0, 0, 0, woodImage.width, woodImage.height, GL_RGBA, GL_UNSIGNED_BYTE, woodImage.data());

	glGenerateTextureMipmap(m_woodTextureID);

	ImageRGBA robotImage = ImageFromFile("Assets/Robot_Texture.png");
	glCreateTextures(GL_TEXTURE_2D, 1, &m_robotTextureID);
	glTextureStorage2D(m_robotTextureID, NumberOfMIPLevels(robotImage), GL_RGBA8, robotImage.width, robotImage.height);
	glTextureSubImage2D(m_robotTextureID, 0, 0, 0, robotImage.width, robotImage.height, GL_RGBA, GL_UNSIGNED_BYTE, robotImage.data());
	glGenerateTextureMipmap(m_robotTextureID);
}

void CMyApp::CleanTextures()
{
	glDeleteTextures(1, &m_woodTextureID);

	glDeleteSamplers( 1, &m_SamplerID );
}

bool CMyApp::Init()
{
	SetupDebugCallback();

	// törlési szín legyen kékes
	glClearColor(0.125f, 0.25f, 0.5f, 1.0f);

	InitShaders();
	InitGeometry();
	InitTextures();

	//
	// egyéb inicializálás
	//

	glEnable(GL_CULL_FACE); // kapcsoljuk be a hátrafelé néző lapok eldobását 
	glCullFace(GL_BACK);    // GL_BACK: a kamerától "elfelé" néző lapok, GL_FRONT: a kamera felé néző lapok 

	glEnable(GL_DEPTH_TEST); // mélységi teszt bekapcsolása (takarás) 

	// kamera 
	m_camera.SetView(
		glm::vec3(0.0, 60.0, 150.0),  // honnan nézzük a színteret  - eye
		glm::vec3(0.0, 0.0, 0.0),  // a színtér melyik pontját nézzük  - at
		glm::vec3(0.0, 1.0, 0.0)); // felfelé mutató irány a világban  - up

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

	if (m_IsPicking) {
		// sugár indítása a kattintott pixelen át 
		Ray ray = CalculatePixelRay(glm::vec2(m_PickedPixel.x, m_PickedPixel.y));
		
        m_IsPicking = false;
	}

	m_cameraManipulator.Update(updateInfo.DeltaTimeInSec);
}

void CMyApp::SetCommonUniforms()
{
	// - Uniform paraméterek 

	// - view és projekciós mátrix 
	glUniformMatrix4fv( ul("viewProj"), 1, GL_FALSE, glm::value_ptr(m_camera.GetViewProj()));

	// - Fényforrások beállítása 
	glUniform3fv( ul("cameraPosition"), 1, glm::value_ptr(m_camera.GetEye()));
	//glUniform4fv( ul("lightPosition"), 1, glm::value_ptr(m_lightPosition));
	//
	//glUniform3fv( ul("La"), 1, glm::value_ptr(m_La));
	//glUniform3fv( ul("Ld"), 1, glm::value_ptr(m_Ld));
	//glUniform3fv( ul("Ls"), 1, glm::value_ptr(m_Ls));
	//
	//glUniform1f( ul("lightConstantAttenuation"), m_lightConstantAttenuation);
	//glUniform1f( ul("lightLinearAttenuation"), m_lightLinearAttenuation);
	//glUniform1f( ul("lightQuadraticAttenuation"), m_lightQuadraticAttenuation);
}

void CMyApp::DrawObject(OGLObject& obj, const glm::mat4& world)
{
	glUniformMatrix4fv( ul("world"), 1, GL_FALSE, glm::value_ptr(world));
	glUniformMatrix4fv( ul("worldInvTransp"), 1, GL_FALSE, glm::value_ptr(glm::transpose(glm::inverse(world))));
	glBindVertexArray(obj.vaoID);
	glDrawElements(GL_TRIANGLES, obj.count, GL_UNSIGNED_INT, nullptr);
}

void CMyApp::Render()
{
	// töröljük a framepuffert (GL_COLOR_BUFFER_BIT)...
	// ... és a mélységi Z puffert (GL_DEPTH_BUFFER_BIT)
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glUseProgram(m_programID);

	SetCommonUniforms();

	glUniform1i( ul("textureImage"), 0);

	glBindTextureUnit(0, m_woodTextureID);
	glBindSampler(0, m_SamplerID);

	glUniform1i(ul("objectType"), 1);

    //DrawObject( m_cubeGPU, glm::identity<glm::mat4>() );
	glm::mat4 tableTopTransform = glm::translate(glm::vec3(0.0f, -5.0f, 0.0f)) * glm::scale(glm::vec3(200.0f, 10.0f, 200.0f));
	DrawObject(m_cubeGPU, tableTopTransform);

	glUniform1i(ul("objectType"), 0);

	glm::vec3 legSize(20.0f, 120.0f, 20.0f);
	float legYOffset = -70.0f;

	glm::mat4 leg1Transform = glm::translate(glm::vec3(60.0f, legYOffset, 60.0f)) * glm::scale(glm::vec3(legSize));
	DrawObject(m_cubeGPU, leg1Transform);

	glm::mat4 leg2Transform = glm::translate(glm::vec3(-60.0f, legYOffset, 60.0f)) * glm::scale(glm::vec3(legSize));
	DrawObject(m_cubeGPU, leg2Transform);

	glm::mat4 leg3Transform = glm::translate(glm::vec3(60.0f, legYOffset, -60.0f)) * glm::scale(glm::vec3(legSize));
	DrawObject(m_cubeGPU, leg3Transform);

	glm::mat4 leg4Transform = glm::translate(glm::vec3(-60.0f, legYOffset, -60.0f)) * glm::scale(glm::vec3(legSize));
	DrawObject(m_cubeGPU, leg4Transform);

	// --- ROBOT KIRAJZOLÁSA ---

	// Textúra váltása a robotéra
	glBindTextureUnit(0, m_robotTextureID);

	// 1. TEST (Torso) - Ő a "fő szülő". Csak fel kell tolni a helyére.
	glm::mat4 torsoTransform = glm::translate(glm::vec3(0.000f, 17.525f, 0.000f));
	DrawObject(m_torsoGPU, torsoTransform);

	// 2. FEJ - Szülője a Test. Forgatás X, majd Y tengely körül a leírás alapján.
	glm::mat4 headTransform = torsoTransform
		* glm::translate(glm::vec3(0.000f, 5.928f, -0.822f))
		* glm::rotate(m_headAngleY, glm::vec3(0.0f, 1.0f, 0.0f))
		* glm::rotate(m_headAngleX, glm::vec3(1.0f, 0.0f, 0.0f));
	DrawObject(m_headGPU, headTransform);

	// 3. BAL KAR (Felkar és Alkar)
	glm::mat4 leftUpperArmTransform = torsoTransform
		* glm::translate(glm::vec3(5.402f, 2.966f, -0.878f))
		* glm::rotate(m_leftUpperArmAngle, glm::vec3(1.0f, 0.0f, 0.0f));
	DrawObject(m_leftUpperArmGPU, leftUpperArmTransform);

	// Az alkar szülője a felkar (ezért a leftUpperArmTransform-mal szorzunk!)
	glm::mat4 leftLowerArmTransform = leftUpperArmTransform
		* glm::translate(glm::vec3(0.114f, -4.633f, -0.005f))
		* glm::rotate(m_leftLowerArmAngle, glm::vec3(1.0f, 0.0f, 0.0f));
	DrawObject(m_leftLowerArmGPU, leftLowerArmTransform);

	// 4. JOBB KAR (Felkar és Alkar)
	glm::mat4 rightUpperArmTransform = torsoTransform
		* glm::translate(glm::vec3(-5.402f, 2.966f, -0.878f))
		* glm::rotate(m_rightUpperArmAngle, glm::vec3(1.0f, 0.0f, 0.0f));
	DrawObject(m_rightUpperArmGPU, rightUpperArmTransform);

	glm::mat4 rightLowerArmTransform = rightUpperArmTransform
		* glm::translate(glm::vec3(-0.114f, -4.633f, -0.005f))
		* glm::rotate(m_rightLowerArmAngle, glm::vec3(1.0f, 0.0f, 0.0f));
	DrawObject(m_rightLowerArmGPU, rightLowerArmTransform);

	// 5. LÁBAK ÉS ANIMÁCIÓ
	// Az animOffset az egyik lábra 0, a másikra π (ami kb. 3.1415)
	glm::vec3 leftLegAnim = CalculateLegAnim(m_ElapsedTimeInSec, 0.0f);
	glm::mat4 leftLegTransform = torsoTransform
		* glm::translate(glm::vec3(2.867f, -4.883f, -0.771f) + leftLegAnim);
	DrawObject(m_legGPU, leftLegTransform);

	glm::vec3 rightLegAnim = CalculateLegAnim(m_ElapsedTimeInSec, glm::pi<float>());
	glm::mat4 rightLegTransform = torsoTransform
		* glm::translate(glm::vec3(-2.867f, -4.883f, -0.771f) + rightLegAnim);
	DrawObject(m_legGPU, rightLegTransform);

	glBindTextureUnit(0, 0);
	glBindSampler(0, 0);

	glBindVertexArray(0);
	// shader kikapcsolása 
	glUseProgram(0);
}

void CMyApp::RenderGUI()
{
	ImGui::Begin("Robot Vezerlo");

	ImGui::SliderAngle("Fej X (Le/Fel)", &m_headAngleX, -45.0f, 45.0f);
	ImGui::SliderAngle("Fej Y (Jobbra/Balra)", &m_headAngleY, -45.0f, 45.0f);

	ImGui::Separator();

	// Karok forgatása a saját X tengelyük körül
	ImGui::SliderAngle("Bal Felkar", &m_leftUpperArmAngle, -90.0f, 90.0f);
	ImGui::SliderAngle("Bal Alkar", &m_leftLowerArmAngle, -90.0f, 0.0f); // A könyök csak egy irányba hajlik
	ImGui::SliderAngle("Jobb Felkar", &m_rightUpperArmAngle, -90.0f, 90.0f);
	ImGui::SliderAngle("Jobb Alkar", &m_rightLowerArmAngle, -90.0f, 0.0f);

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
			glGetIntegerv( GL_POLYGON_MODE, polygonModeFrontAndBack ); // Kérdezzük le a jelenlegi polygon módot! Külön adja a front és back módokat. 
			GLenum polygonMode = ( polygonModeFrontAndBack[ 0 ] != GL_FILL ? GL_FILL : GL_LINE ); // Váltogassuk FILL és LINE között! 
			// https://registry.khronos.org/OpenGL-Refpages/gl4/html/glPolygonMode.xhtml
			glPolygonMode( GL_FRONT_AND_BACK, polygonMode ); // Állítsuk be az újat! 
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
	if ( m_IsCtrlDown )
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