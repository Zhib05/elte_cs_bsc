#include "MyApp.h"
#include "SDL_GLDebugMessageCallback.h"

#include <imgui.h>

CMyApp::CMyApp()
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
		glDebugMessageControl(GL_DONT_CARE, GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR, GL_DONT_CARE, 0, nullptr, GL_FALSE);
		glDebugMessageControl(GL_DONT_CARE, GL_DONT_CARE, GL_DEBUG_SEVERITY_NOTIFICATION, 0, nullptr, GL_FALSE);
		glDebugMessageCallback(SDL_GLDebugMessageCallback, nullptr);
	}
}

bool CMyApp::Init()
{
	SetupDebugCallback();

	// törlési szín legyen kékes
	glClearColor(0.125f, 0.25f, 0.5f, 1.0f);

	//
	// egyéb inicializálás
	//

	glEnable(GL_CULL_FACE); // kapcsoljuk be a hátrafelé néző lapok eldobását 
	glCullFace(GL_BACK);    // GL_BACK: a kamerától "elfelé" néző lapok, GL_FRONT: a kamera felé néző lapok 

	glEnable(GL_DEPTH_TEST); // mélységi teszt bekapcsolása (takarás) 

	return true;
}

void CMyApp::Clean()
{
}

void CMyApp::Update( const SUpdateInfo& updateInfo )
{
	m_ElapsedTimeInSec = updateInfo.ElapsedTimeInSec;
	if (m_animateColor)
		m_clearColor.r = 0.5f + 0.5f * sin(m_ElapsedTimeInSec * 3.14159f);
}

void CMyApp::Render()
{
	// töröljük a framepuffert (GL_COLOR_BUFFER_BIT)...
	// ... és a mélységi Z puffert (GL_DEPTH_BUFFER_BIT)
	glClearColor(m_clearColor.r, m_clearColor.g, m_clearColor.b, 1.0);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

}

void CMyApp::RenderGUI()
{
	if (ImGui::Begin("My Window"))
	{
		ImGui::Text("My Label");
		ImGui::Checkbox("Animate Color", &m_animateColor);

		ImGui::ColorEdit3("Change Color", &m_clearColor.b);
		//ImGui::ColorPicker3("Color Picker", &m_clearColor.x);
		//ImGui::SliderFloat("Red", &m_clearColor.r, 0.0f, 1.0f);

		if (ImGui::Button("Reset Color"))
		{
			m_clearColor = glm::vec3(0.f);
		}
		
		if (ImGui::BeginListBox("##list", ImVec2(-1, 150)))
		{
			for (int i = 0; i < m_clickCount; ++i)
			{
				ImGui::Text("%.0f, %.0f", m_mouseClicks[i].x, m_mouseClicks[i].y);
			}
			ImGui::EndListBox();
		}

		if (ImGui::Button("Delete"))
		{
			m_clickCount = 0;
		}
	}
	ImGui::End();
}

// https://wiki.libsdl.org/SDL2/SDL_KeyboardEvent
// https://wiki.libsdl.org/SDL2/SDL_Keysym
// https://wiki.libsdl.org/SDL2/SDL_Keycode
// https://wiki.libsdl.org/SDL2/SDL_Keymod

void CMyApp::KeyboardDown(const SDL_KeyboardEvent& key)
{	
}

void CMyApp::KeyboardUp(const SDL_KeyboardEvent& key)
{
}

// https://wiki.libsdl.org/SDL2/SDL_MouseMotionEvent

void CMyApp::MouseMove(const SDL_MouseMotionEvent& mouse)
{
	if (m_animateColor)
	{
		m_clearColor.g = mouse.x / (float)m_windowResolution.x;
		m_clearColor.b = mouse.y / (float)m_windowResolution.y;
	}
}

// https://wiki.libsdl.org/SDL2/SDL_MouseButtonEvent

void CMyApp::MouseDown(const SDL_MouseButtonEvent& mouse)
{
	if (mouse.button == SDL_BUTTON_LEFT && m_clickCount < 50)
	{
		m_mouseClicks[m_clickCount].x = mouse.x;
		m_mouseClicks[m_clickCount].y = mouse.y;
		m_clickCount++;
	}
}

void CMyApp::MouseUp(const SDL_MouseButtonEvent& mouse)
{
}

// https://wiki.libsdl.org/SDL2/SDL_MouseWheelEvent

void CMyApp::MouseWheel(const SDL_MouseWheelEvent& wheel)
{
}


// a két paraméterben az új ablakméret szélessége (_w) és magassága (_h) található
void CMyApp::Resize(int _w, int _h)
{
	glViewport(0, 0, _w, _h);
	m_windowResolution = glm::ivec2(_w, _h);
}

// Le nem kezelt, egzotikus esemény kezelése
// https://wiki.libsdl.org/SDL2/SDL_Event

void CMyApp::OtherEvent( const SDL_Event& ev )
{

}