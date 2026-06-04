#version 430

// Pipeline-ból bejövő per-fragment attribútumok 
in vec3 worldPosition;
in vec3 worldNormal;
in vec2 textureCoords;

// Kimenő érték - a fragment színe 
out vec4 outputColor;

// Textúra mintavételező objektum 
uniform sampler2D textureImage;

// Kamera pozíciója
uniform vec3 cameraPosition;

// Fényforrás tulajdonságok (C++ kódból kapjuk)
uniform vec4 lightPosition = vec4( 0, 10, 0, 1 ); 
uniform vec3 La = vec3( 0.1, 0.1, 0.1 );
uniform vec3 Ld = vec3( 1.0, 1.0, 1.0 );
uniform vec3 Ls = vec3( 1.0, 1.0, 1.0 );

// Fényelhalás (Attenuation)
uniform float lightConstantAttenuation    = 1.0;
uniform float lightLinearAttenuation      = 0.0;
uniform float lightQuadraticAttenuation   = 0.001;

// Anyag tulajdonságok (ezeket most fixre vesszük, de C++-ból is jöhetnének)
uniform vec3 Ka = vec3( 1.0 );
uniform vec3 Kd = vec3( 1.0 );
uniform vec3 Ks = vec3( 1.0 );
uniform float Shininess = 20.0;

// GUI kapcsoló (1 = bekapcsolva, 0 = kikapcsolva)
uniform int shadingEnabled = 1;

// Objektum típusa és sorszáma a színezéshez
uniform int objectType = 0;
uniform int colorIndex = 0;
uniform int cardType = 0;

// --- 1. STRUKTÚRÁK (A minta alapján) ---

struct LightProperties
{
	vec4 pos;
	vec3 La;
	vec3 Ld;
	vec3 Ls;
	float constantAttenuation;
	float linearAttenuation;
	float quadraticAttenuation;
};

struct MaterialProperties
{
	vec3 Ka;
	vec3 Kd;
	vec3 Ks;
	float Shininess;
};

// --- 2. VILÁGÍTÁST SZÁMÍTÓ FÜGGVÉNY (A minta alapján) ---

vec3 lighting(LightProperties light, vec3 position, vec3 normal, MaterialProperties material)
{
	vec3 ToLight;
	float Attenuation = 1.0; // Fényelhalás alapértéke
	
	if ( light.pos.w == 0.0 ) // Irány fényforrás (pl. Nap)
	{
		ToLight	= light.pos.xyz;
	}
	else // Pont fényforrás (pl. villanykörte)
	{
		ToLight	= light.pos.xyz - position;
		float LightDistance = length(ToLight);
		Attenuation = 1.0 / ( light.constantAttenuation + light.linearAttenuation * LightDistance + light.quadraticAttenuation * LightDistance * LightDistance);
	}
	
	// Normalizáljuk az irányt (1 egység hosszú vektor)
	ToLight = normalize(ToLight);
	
	// Ambiens (alap) komponens 
	vec3 Ambient = light.La * material.Ka;

	// Diffúz (szórt) komponens 
	float DiffuseFactor = max(dot(ToLight,normal), 0.0) * Attenuation;
	vec3 Diffuse = DiffuseFactor * light.Ld * material.Kd;
	
	// Spekuláris (csillogó) komponens 
	vec3 viewDir = normalize( cameraPosition - position );
	vec3 reflectDir = reflect( -ToLight, normal );
	
	float SpecularFactor = pow(max( dot( viewDir, reflectDir) ,0.0), material.Shininess) * Attenuation;
	vec3 Specular = SpecularFactor * light.Ls * material.Ks;
	
	return Ambient + Diffuse + Specular;
}

// --- 3. FŐPROGRAM ---

void main()
{
	// Készítünk egy másolatot a koordinátákról, amit szabadon módosíthatunk
	vec2 currentTexCoords = textureCoords;

	// --- 2.3 Kártya textúra koordináta eltolás ---
	if (objectType == 2) {
		// Az U (vagy x) koordinátát elosztjuk 5-tel, majd hozzáadjuk a típus ötödét
		currentTexCoords.x = (currentTexCoords.x / 5.0) + (float(cardType) / 5.0);
	}

	// Eredeti textúra színének kiolvasása már a MÓDOSÍTOTT koordinátákkal
	vec4 texColor = texture(textureImage, currentTexCoords);

	// --- 2.2 Zseton színezés ---
	if (objectType == 1) {
		// A modulo (maradékos osztás) 6 miatt a c értéke mindig 0 és 5 között lesz
		int c = colorIndex % 6;
		vec3 orig = texColor.rgb; // Eltároljuk az eredeti R, G, B értékeket
		
		// Kicseréljük a színcsatornákat a feladat táblázata alapján
		if      (c == 0) texColor.rgb = vec3(orig.r, orig.g, orig.b);
		else if (c == 1) texColor.rgb = vec3(orig.g, orig.r, orig.b);
		else if (c == 2) texColor.rgb = vec3(orig.b, orig.g, orig.r);
		else if (c == 3) texColor.rgb = vec3(orig.r, orig.r, orig.b);
		else if (c == 4) texColor.rgb = vec3(orig.r, orig.g, orig.r);
		else if (c == 5) texColor.rgb = vec3(orig.g, orig.r, orig.r);
	}

	// Ha kikapcsoltuk az árnyalást a GUI-n, akkor csak a textúrát adjuk vissza
	if (shadingEnabled == 0) {
		outputColor = texColor;
		return;
	}

	// Normálvektor normalizálása
	vec3 normal = normalize( worldNormal );

	// Fényforrás adatainak betöltése a struktúrába
	LightProperties light;
	light.pos = lightPosition;
	light.La = La;
	light.Ld = Ld;
	light.Ls = Ls;
	light.constantAttenuation = lightConstantAttenuation;
	light.linearAttenuation = lightLinearAttenuation;
	light.quadraticAttenuation = lightQuadraticAttenuation;

	// Anyag adatainak betöltése a struktúrába
	MaterialProperties material;
	material.Ka = Ka;
	material.Kd = Kd;
	material.Ks = Ks;
	material.Shininess = Shininess;

	// Világítás kiszámítása a segédfüggvénnyel
	vec3 shadedColor = lighting(light, worldPosition, normal, material);

	// Végeredmény: A textúra színét megszorozzuk a kiszámolt fény színével
	outputColor = vec4(shadedColor, 1.0) * texColor;
}