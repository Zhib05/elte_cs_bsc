#include "MyApp.h"
#include "SDL_GLDebugMessageCallback.h"

#include <imgui.h>

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
	AttachShader( m_programID, GL_VERTEX_SHADER, "Shaders/Vert_PosTex.vert" );
	AttachShader( m_programID, GL_FRAGMENT_SHADER, "Shaders/Frag_PosTex.frag" );
	LinkProgram( m_programID );
}

void CMyApp::CleanShaders()
{
	glDeleteProgram( m_programID );
}

void CMyApp::InitGeometry()
{
	// Piramis 

	MeshObject<VertexPosTex> pyramidMeshCPU;

	static constexpr float SQRT_2 = glm::root_two<float>();

	pyramidMeshCPU.vertexArray = 
	{
		{ glm::vec3(-1.0f,    0.0f, -1.0f), glm::vec2( 0.0f, 0.0f ) },
		{ glm::vec3( 1.0f,    0.0f, -1.0f), glm::vec2( 1.0f, 0.0f ) },
		{ glm::vec3(-1.0f,    0.0f,  1.0f), glm::vec2( 0.0f, 1.0f ) },
		{ glm::vec3( 1.0f,    0.0f,  1.0f), glm::vec2( 1.0f, 1.0f ) },
		{ glm::vec3( 0.0f,  SQRT_2,  0.0f), glm::vec2( 0.5, 1.0f ) },
		{ glm::vec3( 0.0f,  SQRT_2,  0.0f), glm::vec2( 0.5, 1.0f ) },
		{ glm::vec3( 0.0f,  SQRT_2,  0.0f), glm::vec2( 0.5, 1.0f ) },
		{ glm::vec3( 0.0f,  SQRT_2,  0.0f), glm::vec2( 0.5, 1.0f ) },
		{ glm::vec3( 1.0f,    0.0f, -1.0f), glm::vec2( 0.0f, 0.0f ) },
		{ glm::vec3(-1.0f,    0.0f, -1.0f), glm::vec2( 1.0f, 0.0f ) },
		{ glm::vec3(-1.0f,    0.0f, -1.0f), glm::vec2( 0.0f, 0.0f ) },
		{ glm::vec3(-1.0f,    0.0f,  1.0f), glm::vec2( 1.0f, 0.0f ) },
		{ glm::vec3(-1.0f,    0.0f,  1.0f), glm::vec2( 0.0f, 0.0f ) },
		{ glm::vec3( 1.0f,    0.0f,  1.0f), glm::vec2( 1.0f, 0.0f ) },
		{ glm::vec3( 1.0f,    0.0f,  1.0f), glm::vec2( 0.0f, 0.0f ) },
		{ glm::vec3( 1.0f,    0.0f, -1.0f), glm::vec2( 1.0f, 0.0f ) }
	};

	pyramidMeshCPU.indexArray =
	{
		// 1. háromszög 
		0,1,2,
		// 2. háromszög 
		2,1,3,
		// 3. háromszög 
		4,8,9,
		// 4. háromszög 
		5,10,11,
		// 5. háromszög 
		6,12,13,
		// 6. háromszög 
		7,14,15
	};

	const std::initializer_list<VertexAttributeDescriptor> vertexAttribList =
	{
		{ 0, offsetof( VertexPosTex, position ), 3, GL_FLOAT },
		{ 1, offsetof( VertexPosTex, texcoord ), 2, GL_FLOAT },
	};

	m_pyramidGPU = CreateGLObjectFromMesh( pyramidMeshCPU, vertexAttribList );

	//Fence
	MeshObject<VertexPosTex> fenceMeshCPU;
	fenceMeshCPU.vertexArray =
	{
		{ glm::vec3(-1, -1, 0), glm::vec2(0, 0) },
		{ glm::vec3(1, -1, 0), glm::vec2(1, 0) },
		{ glm::vec3(-1, 1, 0), glm::vec2(0, 1) },
		{ glm::vec3(1, 1, 0), glm::vec2(1, 1) }
	};
	fenceMeshCPU.indexArray =
	{
		0, 1, 2,
		2, 1, 3
	};

	m_fenceGPU = CreateGLObjectFromMesh(fenceMeshCPU, vertexAttribList);
}

void CMyApp::CleanGeometry()
{
	CleanOGLObject( m_fenceGPU );
	CleanOGLObject( m_pyramidGPU );
}

void CMyApp::InitTextures()
{
	glCreateSamplers( 1, &m_SamplerID );
	glSamplerParameteri( m_SamplerID, GL_TEXTURE_WRAP_S, GL_REPEAT );
	glSamplerParameteri( m_SamplerID, GL_TEXTURE_WRAP_T, GL_REPEAT );
	glSamplerParameteri( m_SamplerID, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR );
	glSamplerParameteri( m_SamplerID, GL_TEXTURE_MAG_FILTER, GL_LINEAR );

	// piramis 

	// diffúz textúra 
	ImageRGBA Image = ImageFromFile( "Assets/yellowbrick.png" );

	glCreateTextures( GL_TEXTURE_2D, 1, &m_TextureID );
	glTextureStorage2D( m_TextureID, NumberOfMIPLevels( Image ), GL_RGBA8, Image.width, Image.height );
	glTextureSubImage2D( m_TextureID, 0, 0, 0, Image.width, Image.height, GL_RGBA, GL_UNSIGNED_BYTE, Image.data() );

	glGenerateTextureMipmap( m_TextureID );

	//Fence
	Image = ImageFromFile("Assets/fence.png");

	glCreateTextures(GL_TEXTURE_2D, 1, &m_FenceTextureID);
	glTextureStorage2D(m_FenceTextureID, NumberOfMIPLevels(Image), GL_RGBA8, Image.width, Image.height);
	glTextureSubImage2D(m_FenceTextureID, 0, 0, 0, Image.width, Image.height, GL_RGBA, GL_UNSIGNED_BYTE, Image.data());

	glGenerateTextureMipmap(m_FenceTextureID);

	//Fencemask
	Image = ImageFromFile("Assets/fenceMask.png");

	glCreateTextures(GL_TEXTURE_2D, 1, &m_FenceMaskTextureID);
	glTextureStorage2D(m_FenceMaskTextureID, NumberOfMIPLevels(Image), GL_RGBA8, Image.width, Image.height);
	glTextureSubImage2D(m_FenceMaskTextureID, 0, 0, 0, Image.width, Image.height, GL_RGBA, GL_UNSIGNED_BYTE, Image.data());

	glGenerateTextureMipmap(m_FenceMaskTextureID);

	// Sár/Por textúra
	glCreateTextures(GL_TEXTURE_2D, 1, &m_DirtTextureID);
	glTextureStorage2D(m_DirtTextureID, 1, GL_RGBA8, 1, 1);

	const char dirtColor[] = { 80, 50, 20 };

	glTextureSubImage2D(m_DirtTextureID, 0, 0, 0, 1, 1, GL_RGBA, GL_UNSIGNED_BYTE, dirtColor);

	glGenerateTextureMipmap(m_DirtTextureID);
}

void CMyApp::CleanTextures()
{
	glDeleteTextures(1, &m_DirtTextureID);
	glDeleteTextures(1, &m_FenceMaskTextureID);
	glDeleteTextures( 1, &m_FenceTextureID );
	glDeleteTextures( 1, &m_TextureID );
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
		glm::vec3(0.0, 0.0, 5.0),   // honnan nézzük a színteret  - eye
		glm::vec3(0.0, 0.0, 0.0),   // a színtér melyik pontját nézzük  - at
		glm::vec3(0.0, 1.0, 0.0));  // felfelé mutató irány a világban  - up

	m_cameraManipulator.SetStateFromCamera();

	return true;
}

void CMyApp::Clean()
{
	CleanShaders();
	CleanGeometry();
	CleanTextures();
}

void CMyApp::Update( const SUpdateInfo& updateInfo )
{
	m_ElapsedTimeInSec = updateInfo.ElapsedTimeInSec;
	m_cameraManipulator.Update( updateInfo.DeltaTimeInSec );

    m_pyramidWorldTransform = glm::identity<glm::mat4>();
	m_fenceWorldTransform = glm::translate(glm::vec3(0, 1, 2));
}

void CMyApp::Render()
{
	// töröljük a framepuffert (GL_COLOR_BUFFER_BIT)...
	// ... és a mélységi Z puffert (GL_DEPTH_BUFFER_BIT)
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);


	// piramis 

	// shader bekapcsolása 
	glUseProgram(m_programID);
	
	// - Uniform paraméterek
	// view és projekciós mátrix
	glUniformMatrix4fv( ul("viewProj"), 1, GL_FALSE, glm::value_ptr( m_camera.GetViewProj() ) );

	glUniform1i(ul("textureImage"), 0);
	glUniform1i(ul("textureMask"), 1);
	glUniform1i(ul("textureDirt"), 2);

	glUniformMatrix4fv( ul("world"),    1, GL_FALSE, glm::value_ptr( m_pyramidWorldTransform ) );
	

	glUniform1i( ul("useMask"), 0 );

	// - VAO beállítása 
	glBindVertexArray( m_pyramidGPU.vaoID );

	glBindTextureUnit( 0, m_TextureID );
	glBindTextureUnit( 2, m_DirtTextureID );

	glBindSampler( 0, m_SamplerID );
	glBindSampler( 2, m_SamplerID );

	// Rajzolási parancs kiadása 
	glDrawElements( GL_TRIANGLES,    
					m_pyramidGPU.count,
					GL_UNSIGNED_INT, 
					nullptr );   

	// Fence
	glUniformMatrix4fv(ul("world"), 1, GL_FALSE, glm::value_ptr(m_fenceWorldTransform));

	glUniform1i(ul("useMask"), 1);

	glBindVertexArray(m_fenceGPU.vaoID);

	glBindTextureUnit(0, m_FenceTextureID);
	glBindTextureUnit(1, m_FenceMaskTextureID);
	glBindTextureUnit(2, m_DirtTextureID);

	glBindSampler(1, m_SamplerID);

	glDisable(GL_CULL_FACE); // kapcsoljuk ki a hátrafelé néző lapok eldobását
	glDrawElements(GL_TRIANGLES,
		m_fenceGPU.count,
		GL_UNSIGNED_INT,
		nullptr);
	glEnable(GL_CULL_FACE); // kapcsoljuk vissza a hátrafelé néző lapok eldobását


	// shader kikapcsolása 
	glUseProgram( 0 );

	glBindTextureUnit( 0, 0 );
	glBindTextureUnit( 1, 0 );
	glBindTextureUnit( 2, 0 );
	glBindSampler( 0, 0 );
	glBindSampler( 1, 0 );
	glBindSampler( 2, 0 );
	
	// VAO kikapcsolása 
	glBindVertexArray( 0 );
}

void CMyApp::RenderGUI()
{
	// ImGui::ShowDemoWindow();
}

// https://wiki.libsdl.org/SDL3/SDL_KeyboardEvent
// https://wiki.libsdl.org/SDL3/SDL_Keysym
// https://wiki.libsdl.org/SDL3/SDL_Keycode
// https://wiki.libsdl.org/SDL3/SDL_Keymod

void CMyApp::KeyboardDown(const SDL_KeyboardEvent& key)
{	
	if ( !key.repeat ) // Először lett megnyomva 
	{
		if ( key.key == SDLK_F5 && key.mod & SDL_KMOD_CTRL )
		{
			CleanShaders();
			InitShaders();
		}
		if ( key.key == SDLK_F1 )
		{
			GLint polygonModeFrontAndBack[ 2 ] = {};
			// https://registry.khronos.org/OpenGL-Refpages/gl4/html/glGet.xhtml
			glGetIntegerv( GL_POLYGON_MODE, polygonModeFrontAndBack ); // Kérdezzük le a jelenlegi polygon módot! Külön adja a front és back módokat. 
			GLenum polygonMode = ( polygonModeFrontAndBack[ 0 ] != GL_FILL ? GL_FILL : GL_LINE ); // Váltogassuk FILL és LINE között! 
			// https://registry.khronos.org/OpenGL-Refpages/gl4/html/glPolygonMode.xhtml
			glPolygonMode( GL_FRONT_AND_BACK, polygonMode ); // Állítsuk be az újat! 
		}
	}
	m_cameraManipulator.KeyboardDown( key );
}

void CMyApp::KeyboardUp(const SDL_KeyboardEvent& key)
{
	m_cameraManipulator.KeyboardUp( key );
}

// https://wiki.libsdl.org/SDL3/SDL_MouseMotionEvent

void CMyApp::MouseMove(const SDL_MouseMotionEvent& mouse)
{
	m_cameraManipulator.MouseMove( mouse );
}

// https://wiki.libsdl.org/SDL3/SDL_MouseButtonEvent

void CMyApp::MouseDown(const SDL_MouseButtonEvent& mouse)
{
}

void CMyApp::MouseUp(const SDL_MouseButtonEvent& mouse)
{
}

// https://wiki.libsdl.org/SDL3/SDL_MouseWheelEvent

void CMyApp::MouseWheel(const SDL_MouseWheelEvent& wheel)
{
	m_cameraManipulator.MouseWheel( wheel );
}


// a két paraméterben az új ablakméret szélessége (_w) és magassága (_h) található
void CMyApp::Resize(int _w, int _h)
{
	glViewport(0, 0, _w, _h);
	m_camera.SetAspect( static_cast<float>(_w) / _h );
}

// Le nem kezelt, egzotikus esemény kezelése
// https://wiki.libsdl.org/SDL3/SDL_Event

void CMyApp::OtherEvent( const SDL_Event& ev )
{

}