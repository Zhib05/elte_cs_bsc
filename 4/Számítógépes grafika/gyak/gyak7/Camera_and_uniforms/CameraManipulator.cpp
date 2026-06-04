#include "CameraManipulator.h"

#include "Camera.h"

#include <SDL3/SDL.h>

CameraManipulator::CameraManipulator(Camera &camera) : m_rCamera(camera)
{
}

CameraManipulator::~CameraManipulator()
{
}

void CameraManipulator::SetStateFromCamera()
{
	//m_center = m_rCamera.GetAt(); // a kamera n¨¦z¨¦si c¨¦l poz¨ªci¨®ja a kor modell k?z¨¦ppontja lesz
	//glm::vec3 aim = m_center - m_rCamera.GetEye(); // a kamera n¨¦z¨¦si ir¨¢nya a c¨¦lpont fel¨¦ mutat¨® vektor

	//m_distance = glm::length(aim); // a t¨¢vols¨¢g a c¨¦lpont ¨¦s a kamera kozott

	m_eye = m_rCamera.GetEye();

	glm::vec3 aim = m_rCamera.GetAt() - m_eye;

	aim = glm::normalize(aim);

	m_u = atan2f(aim.z, aim.x);
	m_v = acosf(aim.y);
}

void CameraManipulator::Update( float _deltaTime )
{
	glm::vec3 lookDirection(
		cosf(m_u) * sinf(m_v), // x komponens
		cosf(m_v),            // y komponens
		sinf(m_u) * sinf(m_v)  // z komponens
	);

	//glm::vec3 eye = m_center - lookDirection * m_distance;

	glm::vec3 side = glm::cross(lookDirection, m_rCamera.GetWorldUp());
	
	glm::vec3 deltaPosition = (m_goForward * lookDirection + m_goRight * glm::normalize(side)) * 4.f * _deltaTime;
	
	m_eye += deltaPosition;

	glm::vec3 at = m_eye + lookDirection;
	//m_center += deltaPosition;


	//m_rCamera.SetView(eye, m_center, m_rCamera.GetWorldUp());
	m_rCamera.SetView(m_eye, at, m_rCamera.GetWorldUp());
}


void CameraManipulator::KeyboardDown(const SDL_KeyboardEvent& key)
{
	switch ( key.key )
	{
	case SDLK_LSHIFT:
	case SDLK_RSHIFT:
		break;
	case SDLK_W:
		m_goForward = 1.0f;
		break;
	case SDLK_S:
		m_goForward = -1.0f;
		break;
	case SDLK_A:
		m_goRight = -1.0f;
		break;
	case SDLK_D:
		m_goRight = 1.0f;
		break;
	case SDLK_E:
		break;
	case SDLK_Q:
		break;
	}
}

void CameraManipulator::KeyboardUp(const SDL_KeyboardEvent& key)
{
	
	switch ( key.key )
	{
	case SDLK_LSHIFT:
	case SDLK_RSHIFT:
		break;
	case SDLK_W:
	case SDLK_S:
		m_goForward = 0.0f;
		break;
	case SDLK_A:
	case SDLK_D:
		m_goRight = 0.0f;
		break;
	case SDLK_Q:
	case SDLK_E:
		break;
	}
}


void CameraManipulator::MouseMove(const SDL_MouseMotionEvent& mouse)
{
	if ( mouse.state & SDL_BUTTON_LMASK )
	{
		m_u += mouse.xrel / 100.f; // az eger erzekenyseg 100.0f-del osztva, hogy ne legyen tul gyors a forg¨¢s
		m_v = glm::clamp(m_v + mouse.yrel / 100.f, 0.1f, 3.1f); // az eger erzekenyseg 100.0f-del osztva, hogy ne legyen tul gyors a forg¨¢s; a v koordin¨¢t¨¢t 0.1 ¨¦s 3.1 k?z¨¦ korl¨¢tozzuk, hogy ne forduljon meg a kamera
	}
	if ( mouse.state & SDL_BUTTON_RMASK )
	{
	}
}

void CameraManipulator::MouseWheel(const SDL_MouseWheelEvent& wheel)
{
}