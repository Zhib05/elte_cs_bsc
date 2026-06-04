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
#include "Camera.h"
#include "CameraManipulator.h"
#include "GLUtils.hpp"

struct SUpdateInfo
{
	float ElapsedTimeInSec = 0.0f; // Program indulása óta eltelt idő 
	float DeltaTimeInSec   = 0.0f; // Előző Update óta eltelt idő 
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


	// Kiválasztás 

	glm::ivec2 m_PickedPixel = glm::ivec2( 0, 0 );
	bool m_IsPicking = false;
	bool m_IsCtrlDown = false;

	glm::uvec2 m_windowSize = glm::uvec2(0, 0);

	Ray CalculatePixelRay(glm::vec2 pickerPos) const;


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

	// Geometriával kapcsolatos változók 

	void SetCommonUniforms();
	void DrawObject(OGLObject& obj, const glm::mat4& world);

	OGLObject m_cubeGPU = {};

	// Robot testrészei
	OGLObject m_headGPU = {};
	OGLObject m_torsoGPU = {};
	OGLObject m_leftUpperArmGPU = {};
	OGLObject m_leftLowerArmGPU = {};
	OGLObject m_rightUpperArmGPU = {};
	OGLObject m_rightLowerArmGPU = {};
	OGLObject m_legGPU = {};

	// GUI Csúszkákhoz (Sliderekhez) tartozó szögek radiánban
	float m_headAngleX = 0.0f;
	float m_headAngleY = 0.0f;
	float m_leftUpperArmAngle = 0.0f;
	float m_leftLowerArmAngle = 0.0f;
	float m_rightUpperArmAngle = 0.0f;
	float m_rightLowerArmAngle = 0.0f;

	// Geometria inicializálása, és törlése 
	void InitGeometry();
	void CleanGeometry();

	// Textúrázás, és változói 
	GLuint m_SamplerID = 0;

	GLuint m_woodTextureID = 0;
	GLuint m_robotTextureID = 0;

	void InitTextures();
	void CleanTextures();
};