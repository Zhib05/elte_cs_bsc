#pragma once

// GLM
#include <glm/glm.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtx/transform.hpp>

// GLEW
#include <GL/glew.h>

// SDL
#include <SDL3/SDL.h>
#include <SDL3/SDL_opengl.h>

// Utils
#include "GLUtils.hpp"
#include "Camera.h"
#include "CameraManipulator.h"

struct SUpdateInfo
{
	float ElapsedTimeInSec = 0.0f; // Program indulása óta eltelt idő 
	float DeltaTimeInSec = 0.0f;   // Előző Update óta eltelt idő 
};

class CMyApp
{
public:
	CMyApp();
	~CMyApp();

	bool Init();
	void Clean();

	void Update(const SUpdateInfo&);
	void Render();
	void RenderGUI();

	void KeyboardDown(const SDL_KeyboardEvent&);
	void KeyboardUp(const SDL_KeyboardEvent&);
	void MouseMove(const SDL_MouseMotionEvent&);
	void MouseDown(const SDL_MouseButtonEvent&);
	void MouseUp(const SDL_MouseButtonEvent&);
	void MouseWheel(const SDL_MouseWheelEvent&);
	void Resize(int, int);

	void OtherEvent(const SDL_Event&);

protected:
	void SetupDebugCallback();

	//
	// Adat változók
	//

	float m_ElapsedTimeInSec = 0.0f;

	glm::mat4 m_objectWorldTransform;

	// Object params

	struct FishState {
		glm::vec3 position;
		float rot_Angle;
	};

	static constexpr glm::vec3 OBJECT_POS = glm::vec3( 0.0f, 0.0f, 0.0f );

	std::vector<FishState> m_fishes;

	int fishCount = 5;

	const int SHADER_STATE_DEFAULT = 0;
	const int SHADER_STATE_TALAJ = 1;
	const int SHADER_STATE_OCEAN = 2;

	// Picking

	glm::ivec2 m_PickedPixel = glm::ivec2( 0, 0 );
	bool m_IsPicking = false;
	bool m_IsCtrlDown = false;

	glm::uvec2 m_windowSize = glm::uvec2(0, 0);

	Ray CalculatePixelRay(glm::vec2 pickerPos) const;

	void DrawObject(OGLObject& obj, const glm::mat4& world);

	float m_armR = 0.0f;
	float m_clawR = 0.0f;

	bool m_isWarningLightOn = true;

	glm::mat4 subBaseT;
	glm::mat4 armT;
	glm::mat4 rClawT;
	glm::mat4 lClawT;

	// Kamera

	Camera m_camera;
	CameraManipulator m_cameraManipulator;

	//
	// OpenGL-es dolgok
	//

	// Shaderekhez szükséges változók 
	GLuint m_programID = 0; // shaderek programja 

	// Shaderek inicializálása, és törlése
	void InitShaders();
	void CleanShaders();

	// Fényforrás - ...
	glm::vec4 m_lightPosition = glm::vec4(0.0f, 1.0f, 0.0f, 0.0f);

	glm::vec3 m_La = glm::vec3(0.0, 0.0, 0.0);
	glm::vec3 m_Ld = glm::vec3(1.0, 1.0, 1.0);
	glm::vec3 m_Ls = glm::vec3(1.0, 1.0, 1.0);

	float m_lightConstantAttenuation = 1.0;
	float m_lightLinearAttenuation = 0.0;
	float m_lightQuadraticAttenuation = 0.0;

	// ...  és anyagjellemzők 
	glm::vec3 m_Ka = glm::vec3(1.0);
	glm::vec3 m_Kd = glm::vec3(1.0);
	glm::vec3 m_Ks = glm::vec3(1.0);

	float m_Shininess = 32.0;

	// Geometriával kapcsolatos változók

	void SetCommonUniforms();

	OGLObject m_quadGPU = {};
	OGLObject m_fishGPU = {};

	OGLObject m_subGPU = {};
	OGLObject m_armGPU = {};
	OGLObject m_clawGPU = {};

	// Geometria inicializálása, és törlése
	void InitGeometry();
	void CleanGeometry();

	void RenderTalaj();
	void RenderOcean();
	void RenderFish();
	void RenderSub();

	// Textúrázás, és változói
	GLuint m_SamplerID = 0;

	GLuint m_talajTextureID = 0;
	GLuint m_oceanTextureID = 0;
	GLuint m_fishTextureID = 0;

	GLuint m_subTextureID = 0;
	GLuint m_armTextureID = 0;
	GLuint m_clawTextureID = 0;

	void InitTextures();
	void CleanTextures();
};