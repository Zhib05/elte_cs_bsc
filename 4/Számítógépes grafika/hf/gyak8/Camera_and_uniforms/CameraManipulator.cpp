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
	m_center = m_rCamera.GetAt();
	glm::vec3 aim = m_center - m_rCamera.GetEye();

	m_distance = glm::length(aim);

	m_u = atan2f(aim.z, aim.x);
	m_v = acosf(aim.y);
}

void CameraManipulator::Update( float _deltaTime )
{
	glm::vec3 lookDirection(
		cosf(m_u) * sinf(m_v),
		cosf(m_v),
		sinf(m_u) * sinf(m_v)
	);

	glm::vec3 eye = m_center - lookDirection * m_distance;

	glm::vec3 side = glm::cross(m_rCamera.GetWorldUp(), lookDirection);

	glm::vec3 deltaPosition = (m_goForward * lookDirection + m_goSide * side) * 4.f * _deltaTime;

	eye += deltaPosition;
	m_center += deltaPosition;

	m_rCamera.SetView(eye, m_center, m_rCamera.GetWorldUp());
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
		m_goSide = 1.0f;
		break;
	case SDLK_D:
		m_goSide = -1.0f;
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
		m_goSide = 0.0f;
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
		m_u += mouse.xrel / 100.f;
		m_v = glm::clamp(m_v + mouse.yrel / 100.f, 0.1f, 3.1f);
	}
	if ( mouse.state & SDL_BUTTON_RMASK )
	{
	}
}

void CameraManipulator::MouseWheel(const SDL_MouseWheelEvent& wheel)
{
	m_distance *= powf(0.9f, static_cast<float>(wheel.y));
}