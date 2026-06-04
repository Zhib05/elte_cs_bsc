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

	int chipN = 10;
	int chipK = 5;

	struct CardState {
		glm::vec3 position;
		int type;
	};

	// Object params

	static constexpr glm::vec3 OBJECT_POS = glm::vec3( 0.0f, 0.0f, 0.0f );

	std::vector<CardState> m_cards;
	float m_handRotation = 0.0f;

	// Picking

	glm::ivec2 m_PickedPixel = glm::ivec2( 0, 0 );
	bool m_IsPicking = false;
	bool m_IsCtrlDown = false;

	glm::uvec2 m_windowSize = glm::uvec2(0, 0);

	Ray CalculatePixelRay(glm::vec2 pickerPos) const;

	void DrawObject(OGLObject& obj, const glm::mat4& world);


	// Kamera

	Camera m_camera;
	CameraManipulator m_cameraManipulator;

	//
	// OpenGL-es dolgok
	//

	// Shaderekhez szükséges változók 
	GLuint m_programID = 0; // shaderek programja 

	// Állapot kapcsolók
	bool m_lightMoving = true;
	bool m_shadingEnabled = true;

	// A fény saját idomérője (hogy megállítható legyen anélkül, hogy a játékidő megállna)
	float m_lightTime = 0.0f;

	// Fényforrás - ...
	glm::vec4 m_lightPosition = glm::vec4(0.0f, 0.0f, 0.0f, 1.0f);

	glm::vec3 m_La = glm::vec3(0.1f, 0.1f, 0.1f);
	glm::vec3 m_Ld = glm::vec3(1.0f, 1.0f, 1.0f);
	glm::vec3 m_Ls = glm::vec3(1.0f, 1.0f, 1.0f);

	float m_lightConstantAttenuation = 1.0;
	float m_lightLinearAttenuation = 0.0;
	float m_lightQuadraticAttenuation = 0.001f;

	//// ...  és anyagjellemzők 
	//glm::vec3 m_Ka = glm::vec3(1.0);
	//glm::vec3 m_Kd = glm::vec3(1.0);
	//glm::vec3 m_Ks = glm::vec3(1.0);

	//float m_Shininess = 32.0;

	// Shaderek inicializálása, és törlése
	void InitShaders();
	void CleanShaders();

	// Geometriával kapcsolatos változók

	void SetCommonUniforms();

	void RenderTable();
	void RenderCard();
	void RenderHand();
	void RenderChip();

	OGLObject m_quadGPU = {};
	OGLObject m_handGPU = {};
	OGLObject m_chipGPU = {};

	// Geometria inicializálása, és törlése
	void InitGeometry();
	void CleanGeometry();

	// Textúrázás, és változói
	GLuint m_SamplerID = 0;

	GLuint m_tableTextureID = 0;
	GLuint m_cardTextureID = 0;
	GLuint m_handTextureID = 0;
	GLuint m_chipTextureID = 0;

	void InitTextures();
	void CleanTextures();
};