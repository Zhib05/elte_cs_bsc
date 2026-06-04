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
	float DeltaTimeInSec   = 0.0f; // Előző Update óta eltelt idő 
};

class CMyApp
{
public:
	CMyApp();
	~CMyApp();

	bool Init();
	void Clean();

	void Update( const SUpdateInfo& );
	void Render();
	void RenderGUI();

	void KeyboardDown(const SDL_KeyboardEvent&);
	void KeyboardUp(const SDL_KeyboardEvent&);
	void MouseMove(const SDL_MouseMotionEvent&);
	void MouseDown(const SDL_MouseButtonEvent&);
	void MouseUp(const SDL_MouseButtonEvent&);
	void MouseWheel(const SDL_MouseWheelEvent&);
	void Resize(int, int);

	void OtherEvent( const SDL_Event& );
protected:
	void SetupDebugCallback();

	//
	// Adat változók
	//

	float m_ElapsedTimeInSec = 0.0f;

	glm::mat4 m_originObjectWorldTransform;
	glm::mat4 m_objectWorldTransforms[6];
	glm::mat4 m_linearCurveTransform;

	glm::vec3 m_linearControlPoints[2] = {
		glm::vec3(-5, 0, 5),
		glm::vec3(5, 0, -5)
	};
	float m_t = 0;

	glm::vec3 EvalLinearCurve(glm::vec3 p0, glm::vec3 p1, float t);

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

	GLuint  vaoID = 0; // Vertex Array Object erőforrás azonosító 
	GLuint  vboID = 0; // Vertex Buffer Object erőforrás azonosító 
	GLuint  iboID = 0; // Index Buffer Object erőforrás azonosító 
	GLsizei count = 0; // mennyi indexet/vertexet kell rajzolnunk 

	// Geometria inicializálása, és törlése 
	void InitGeometry();
	void CleanGeometry();
};
