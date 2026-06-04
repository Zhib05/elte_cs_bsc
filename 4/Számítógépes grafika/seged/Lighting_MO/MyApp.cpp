#include "MyApp.h"
#include "SDL_GLDebugMessageCallback.h"
#include "ObjParser.h"

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
	AttachShader( m_programID, GL_VERTEX_SHADER, "Shaders/Vert_PosNormTex.vert" );
	AttachShader( m_programID, GL_FRAGMENT_SHADER, "Shaders/Frag_Lighting.frag" );
	LinkProgram( m_programID );
}

void CMyApp::CleanShaders()
{
	glDeleteProgram( m_programID );
}

void CMyApp::InitGeometry()
{

	const std::initializer_list<VertexAttributeDescriptor> vertexAttribList =
	{
		{ 0, offsetof( Vertex, position ), 3, GL_FLOAT },
		{ 1, offsetof( Vertex, normal   ), 3, GL_FLOAT },
		{ 2, offsetof( Vertex, texcoord ), 2, GL_FLOAT },
	};

	// Quad 

	MeshObject<Vertex> quadMeshCPU;

	quadMeshCPU.vertexArray = 
	{
		{ glm::vec3( -1, -1, 0 ),glm::vec3( 0.0, 0.0, 1.0 ), glm::vec2( 0.0, 0.0 ) },
		{ glm::vec3(  1, -1, 0 ),glm::vec3( 0.0, 0.0, 1.0 ), glm::vec2( 1.0, 0.0 ) },
		{ glm::vec3( -1,  1, 0 ),glm::vec3( 0.0, 0.0, 1.0 ), glm::vec2( 0.0, 1.0 ) },
		{ glm::vec3(  1,  1, 0 ),glm::vec3( 0.0, 0.0, 1.0 ), glm::vec2( 1.0, 1.0 ) }
	};

	quadMeshCPU.indexArray =
	{
		0, 1, 2,
		1, 3, 2
	};

	m_quadGPU = CreateGLObjectFromMesh( quadMeshCPU, vertexAttribList );

	// Suzanne

	MeshObject<Vertex> suzanneMeshCPU = ObjParser::parse("Assets/Suzanne.obj");

	m_SuzanneGPU = CreateGLObjectFromMesh( suzanneMeshCPU, vertexAttribList );

	// Gömb 

	MeshObject<Vertex> sphereMeshCPU = ObjParser::parse("Assets/MarbleBall.obj");

	m_sphereGPU = CreateGLObjectFromMesh( sphereMeshCPU, vertexAttribList );
}

void CMyApp::CleanGeometry()
{
	CleanOGLObject( m_quadGPU );
	CleanOGLObject( m_SuzanneGPU );
	CleanOGLObject( m_sphereGPU );
}

void CMyApp::InitTextures()
{
	glCreateSamplers( 1, &m_SamplerID );
	glSamplerParameteri( m_SamplerID, GL_TEXTURE_WRAP_S, GL_REPEAT );
	glSamplerParameteri( m_SamplerID, GL_TEXTURE_WRAP_T, GL_REPEAT );
	glSamplerParameteri( m_SamplerID, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR );
	glSamplerParameteri( m_SamplerID, GL_TEXTURE_MAG_FILTER, GL_LINEAR );

	// diffúz textúra 
	ImageRGBA woodImage = ImageFromFile( "Assets/Wood_Table_Texture.png" );

	glCreateTextures( GL_TEXTURE_2D, 1, &m_woodTextureID );
	glTextureStorage2D( m_woodTextureID, NumberOfMIPLevels( woodImage ), GL_RGBA8, woodImage.width, woodImage.height );
	glTextureSubImage2D( m_woodTextureID, 0, 0, 0, woodImage.width, woodImage.height, GL_RGBA, GL_UNSIGNED_BYTE, woodImage.data() );

	glGenerateTextureMipmap( m_woodTextureID );

	ImageRGBA SuzanneImage = ImageFromFile( "Assets/wood.jpg" );

	glCreateTextures( GL_TEXTURE_2D, 1, &m_SuzanneTextureID );
	glTextureStorage2D( m_SuzanneTextureID, NumberOfMIPLevels( SuzanneImage ), GL_RGBA8, SuzanneImage.width, SuzanneImage.height );
	glTextureSubImage2D( m_SuzanneTextureID, 0, 0, 0, SuzanneImage.width, SuzanneImage.height, GL_RGBA, GL_UNSIGNED_BYTE, SuzanneImage.data() );

	glGenerateTextureMipmap( m_SuzanneTextureID );

	ImageRGBA sphereImage = ImageFromFile( "Assets/MarbleBall.png" );

	glCreateTextures( GL_TEXTURE_2D, 1, &m_sphereTextureID );
	glTextureStorage2D( m_sphereTextureID, NumberOfMIPLevels( sphereImage ), GL_RGBA8, sphereImage.width, sphereImage.height );
	glTextureSubImage2D( m_sphereTextureID, 0, 0, 0, sphereImage.width, sphereImage.height, GL_RGBA, GL_UNSIGNED_BYTE, sphereImage.data() );

	glGenerateTextureMipmap( m_sphereTextureID );
}

void CMyApp::CleanTextures()
{
	glDeleteTextures( 1, &m_woodTextureID );
	glDeleteTextures( 1, &m_SuzanneTextureID );
	glDeleteTextures( 1, &m_sphereTextureID );

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
		glm::vec3(0.0, 7.0, 7.0),   // honnan nézzük a színteret  - eye
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

	// világ transzformációk 

	m_tableWorldTransform = glm::translate( TABLE_POS ) 
		* glm::rotate( glm::half_pi<float>(), glm::vec3( -1.0f,0.0,0.0) )
		* glm::scale( glm::vec3( TABLE_SIZE ) );

    m_sphereWorldTransform = glm::translate( SPHERE_POS );

    m_SuzanneWorldTransform = glm::translate( SUZANNE_POS );

	// Fényforrás 

	if ( m_IsLampOn ) // irány fényforrás 
	{
		glClearColor(0.125f, 0.25f, 0.5f, 1.0f);
		m_lightPosition = glm::vec4( m_lampPos, 0.0f );
		m_La = glm::vec3( 0.125f );
		m_Ld = m_Ls = m_lampColor;
	}
	else // pont fényforrás 
	{
		glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
		m_lightPosition = glm::vec4( m_bugPos, 1.0f );
		m_La = glm::vec3( 0.0125f );
		m_Ld = m_Ls = BUG_COLOR;	
	}
}


void CMyApp::SetCommonUniforms()
{
	// - view és projekciós mátrix 
	glUniformMatrix4fv( ul("viewProj"), 1, GL_FALSE, glm::value_ptr( m_camera.GetViewProj() ) );
	
	// - Fényforrások beállítása 
	glUniform3fv( ul("cameraPosition"), 1, glm::value_ptr( m_camera.GetEye() ) );
	glUniform4fv( ul("lightPosition"), 1, glm::value_ptr( m_lightPosition ) );

	glUniform3fv( ul("La"), 1, glm::value_ptr( m_La ) );
	glUniform3fv( ul("Ld"), 1, glm::value_ptr( m_Ld ) );
	glUniform3fv( ul("Ls"), 1, glm::value_ptr( m_Ls ) );

	glUniform1f( ul("lightConstantAttenuation"), m_lightConstantAttenuation );
	glUniform1f( ul("lightLinearAttenuation"), m_lightLinearAttenuation );
	glUniform1f( ul("lightQuadraticAttenuation"), m_lightQuadraticAttenuation );
}

void CMyApp::DrawObject(OGLObject& obj, const glm::mat4& world) {
	glUniformMatrix4fv( ul("world"), 1, GL_FALSE, glm::value_ptr(world));
	glUniformMatrix4fv( ul("worldInvTransp"), 1, GL_FALSE, glm::value_ptr(glm::transpose(glm::inverse(world))));
	glBindVertexArray(obj.vaoID);
	glDrawElements(GL_TRIANGLES, obj.count, GL_UNSIGNED_INT, nullptr);
}

void CMyApp::RenderTable()
{
	// - Anyagjellemzők beállítása 
	glUniform3fv( ul("Ka"),		 1, glm::value_ptr( m_Ka ) );
	glUniform3fv( ul("Kd"),		 1, glm::value_ptr( m_Kd ) );
	glUniform3fv( ul("Ks"),		 1, glm::value_ptr( m_Ks ) );

	glUniform1f( ul("Shininess"),	m_Shininess );


	// - textúraegységek beállítása 
	glUniform1i( ul("textureImage"), 0 );

	// - Textúrák beállítása, minden egységre külön 
	glBindTextureUnit( 0, m_woodTextureID );
	glBindSampler( 0, m_SamplerID );

    DrawObject( m_quadGPU, m_tableWorldTransform );
}

void CMyApp::RenderBall()
{
	// - Anyagjellemzők beállítása 
	glUniform3fv( ul("Ka"),		 1, glm::value_ptr( m_Ka ) );
	glUniform3fv( ul("Kd"),		 1, glm::value_ptr( m_Kd ) );
	glUniform3fv( ul("Ks"),		 1, glm::value_ptr( m_Ks ) );

	glUniform1f( ul("Shininess"),	m_Shininess );


	// - textúraegységek beállítása 
	glUniform1i( ul("textureImage"), 0 );

	// - Textúrák beállítása, minden egységre külön 
	glBindTextureUnit( 0, m_sphereTextureID );
	glBindSampler( 0, m_SamplerID );


    DrawObject( m_sphereGPU, m_sphereWorldTransform );
}

void CMyApp::RenderSuzanne()
{
	// - Anyagjellemzők beállítása 
	glUniform3fv( ul("Ka"),		 1, glm::value_ptr( m_Ka ) );
	glUniform3fv( ul("Kd"),		 1, glm::value_ptr( m_Kd ) );
	glUniform3fv( ul("Ks"),		 1, glm::value_ptr( m_Ks ) );

	glUniform1f( ul("Shininess"),	m_Shininess );

	// - textúraegységek beállítása 
	glUniform1i( ul("textureImage"), 0 );

	// - Textúrák beállítása, minden egységre külön 
	glBindTextureUnit( 0, m_SuzanneTextureID );
	glBindSampler( 0, m_SamplerID );
	
    DrawObject( m_SuzanneGPU, m_SuzanneWorldTransform );
}

void CMyApp::Render()
{
	// töröljük a framepuffert (GL_COLOR_BUFFER_BIT)...
	// ... és a mélységi Z puffert (GL_DEPTH_BUFFER_BIT)
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glUseProgram( m_programID );

	SetCommonUniforms();

	RenderTable();
    RenderBall();
    RenderSuzanne();

	// shader kikapcsolása 
	glUseProgram( 0 );

	// - Textúrák kikapcsolása, minden egységre külön 
	glBindTextureUnit( 0, 0 );
	glBindSampler( 0, 0 );


	// VAO kikapcsolása 
	glBindVertexArray( 0 );
}

void CMyApp::RenderGUI()
{
	// ImGui::ShowDemoWindow();
	if ( ImGui::Begin( "Lighting settings" ) )
	{
		ImGui::Checkbox("Is lamp on?", &m_IsLampOn );
		ImGui::SeparatorText("Settings");
		
		//ImGui::SliderFloat("Shininess", &m_Shininess, 0.0001f, 10.0f );
		ImGui::InputFloat("Shininess", &m_Shininess, 0.1f, 1.0f, "%.1f" );
		static float Kaf = 1.0f;
		static float Kdf = 1.0f;
		static float Ksf = 1.0f;
		if ( ImGui::SliderFloat( "Ka", &Kaf, 0.0f, 1.0f ) )
		{
			m_Ka = glm::vec3( Kaf );
		}
		if ( ImGui::SliderFloat( "Kd", &Kdf, 0.0f, 1.0f ) )
		{
			m_Kd = glm::vec3( Kdf );
		}
		if ( ImGui::SliderFloat( "Ks", &Ksf, 0.0f, 1.0f ) )
		{
			m_Ks = glm::vec3( Ksf );
		}

		if ( m_IsLampOn )
		{
			static glm::vec2 lampPosXZ = glm::vec2( 0.0f );
			lampPosXZ = glm::vec2( m_lampPos.x, m_lampPos.z );
			if ( ImGui::SliderFloat2( "Lamp Position XZ", glm::value_ptr( lampPosXZ ), -1.0f, 1.0f ) )
			{
				float lampPosL2 = lampPosXZ.x * lampPosXZ.x + lampPosXZ.y * lampPosXZ.y;
				if ( lampPosL2 > 1.0f ) // Ha kívülre esne a körön, akkor normalizáljuk 
				{
					lampPosXZ /= sqrtf( lampPosL2 );
					lampPosL2 = 1.0f;
				}

				m_lampPos.x = lampPosXZ.x;
				m_lampPos.z = lampPosXZ.y;
				m_lampPos.y = sqrtf( 1.0f - lampPosL2 );
			}
			ImGui::LabelText( "Lamp Position Y", "%f", m_lampPos.y );
			ImGui::ColorEdit3( "Lamp color", glm::value_ptr( m_lampColor ), ImGuiColorEditFlags_Float );
		}
		else
		{

			ImGui::SliderFloat( "Constant Attenuation", &m_lightConstantAttenuation, 0.001f, 2.0f );
			ImGui::SliderFloat( "Linear Attenuation", &m_lightLinearAttenuation, 0.001f, 2.0f );
			ImGui::SliderFloat( "Quadratic Attenuation", &m_lightQuadraticAttenuation, 0.001f, 2.0f );

			ImGui::SliderFloat3( "Bug Position", glm::value_ptr( m_bugPos ), -TABLE_SIZE, TABLE_SIZE );
			
		}
		
	}

	ImGui::End();
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