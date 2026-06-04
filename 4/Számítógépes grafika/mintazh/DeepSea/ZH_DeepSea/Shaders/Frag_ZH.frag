#version 430

// pipeline-ból bejövő per-fragment attribútumok 
in vec3 worldPosition;
in vec3 worldNormal;
in vec2 textureCoords;

// kimenő érték - a fragment színe 
out vec4 outputColor;

// textúra mintavételező objektum 
uniform sampler2D textureImage;

uniform vec3 cameraPosition;

uniform int isWorldCoordsTex = 0;
uniform int ObjectType = 0;
uniform float ETimeInSec = 0.0;

// fényforrás tulajdonságok 
uniform vec4 lightPosition = vec4( 0.0, 1.0, 0.0, 0.0);

uniform vec3 La = vec3(0.0, 0.0, 0.0 );
uniform vec3 Ld = vec3(1.0, 1.0, 1.0 );
uniform vec3 Ls = vec3(1.0, 1.0, 1.0 );

uniform float lightConstantAttenuation    = 1.0;
uniform float lightLinearAttenuation      = 0.0;
uniform float lightQuadraticAttenuation   = 0.0;

// 2.3-as feladat: Jelzőfény uniform változói
uniform vec4 light2Position;
uniform vec3 light2Ld;
uniform vec3 light2Ls;
uniform float light2ConstantAttenuation;
uniform float light2LinearAttenuation;
uniform float light2QuadraticAttenuation;
uniform int isLight2On;

// anyag tulajdonságok 

uniform vec3 Ka = vec3( 1.0 );
uniform vec3 Kd = vec3( 1.0 );
uniform vec3 Ks = vec3( 1.0 );

uniform float Shininess = 20.0;

/* segítség:  normalizálás:  http://www.opengl.org/sdk/docs/manglsl/xhtml/normalize.xml
	- skaláris szorzat:   http://www.opengl.org/sdk/docs/manglsl/xhtml/dot.xml
	- clamp: http://www.opengl.org/sdk/docs/manglsl/xhtml/clamp.xml
	- reflect: http://www.opengl.org/sdk/docs/manglsl/xhtml/reflect.xml
			 reflect(beérkező_vektor, normálvektor);  pow(alap, kitevő); */

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

vec3 lighting(LightProperties light, vec3 position, vec3 normal, MaterialProperties material)
{
	
	vec3 ToLight; // A fényforrásBA mutató vektor 
	float Attenuation = 1.0; // Attenuáció (fényelhalás) 
	
	if ( light.pos.w == 0.0 ) // irány fényforrás (directional light) 
	{
		// Irányfényforrás esetén minden pont ugyan abból az irányból van megvilágítva
		ToLight	= light.pos.xyz;

		// Az attenuációt hagyjuk 1-en, hogy ne változtassa a fényt
	}
	else				  // pont fényforrás (positional light) 
	{
		// Pontfényforrás esetén kiszámoljuk a fragment pontból a fényforrásba mutató vektort, ...
		ToLight	= light.pos.xyz - position;
		
		// ... és a távolságot a fényforrástól 
		float LightDistance = length(ToLight);
		
		// ... végül a fényelhalást 
		Attenuation = 1.0 / ( light.constantAttenuation + light.linearAttenuation * LightDistance + light.quadraticAttenuation * LightDistance * LightDistance);
	}
	// Normalizáljuk a fényforrásba mutató vektort 
	ToLight = normalize(ToLight);
	
	// Ambiens komponens 
	// Ambiens fény mindenhol ugyanakkora 
	vec3 Ambient = light.La * material.Ka;

	// Diffúz komponens 
	// A diffúz fényforrásból érkező fény mennyisége arányos a fényforrásba mutató vektor és a normálvektor skaláris szorzatával
	// és az attenuációval
	float DiffuseFactor = max(dot(ToLight,normal), 0.0) * Attenuation;
	vec3 Diffuse = DiffuseFactor * light.Ld * material.Kd;
	
	// Spekuláris komponens 
	vec3 viewDir = normalize( cameraPosition - position ); // A fragmentből a kamerába mutató vektor 
	vec3 reflectDir = reflect( -ToLight, normal ); // Tökéletes visszaverődés vektora 
	
	// A spekuláris komponens a tökéletes visszaverődés iránya és a kamera irányától függ.
	// A koncentráltsága cos()^s alakban számoljuk, ahol s a fényességet meghatározó paraméter.
	// Szintén függ az attenuációtól.
	float SpecularFactor = pow(max( dot( viewDir, reflectDir) ,0.0), material.Shininess) * Attenuation;
	vec3 Specular = SpecularFactor * light.Ls * material.Ks;

	if (ObjectType == 1) {
		return Ambient + Diffuse;
	}

	return Ambient + Diffuse + Specular;
}

void main()
{
	// A fragment normálvektora 
	// MINDIG normalizáljuk! 
	vec3 normal = normalize( worldNormal );

	LightProperties light;
	light.pos = lightPosition;
	light.La = La;
	light.Ld = Ld;
	light.Ls = Ls;
	light.constantAttenuation = lightConstantAttenuation;
	light.linearAttenuation = lightLinearAttenuation;
	light.quadraticAttenuation = lightQuadraticAttenuation;

	MaterialProperties material;
	material.Ka = Ka;
	material.Kd = Kd;
	material.Ks = Ks;
	material.Shininess = Shininess;

	vec3 shadedColor = lighting(light, worldPosition, normal, material);

	float depthY = min(0.0, worldPosition.y);
	vec3 extinction = exp(vec3(0.014, 0.01, 0.004) * depthY);
	
	// A kiszámolt árnyalt színt megszorozzuk az elhalási értékkel
	shadedColor = shadedColor * extinction;

	// 2.3-as feladat: Jelzőfény hozzáadása az elhalás után
	if (isLight2On == 1) {
		LightProperties light2;
		light2.pos = light2Position;
		light2.La = vec3(0.0); // A pontfénynek nincs ambiens komponense
		light2.Ld = light2Ld;
		light2.Ls = light2Ls;
		light2.constantAttenuation = light2ConstantAttenuation;
		light2.linearAttenuation = light2LinearAttenuation;
		light2.quadraticAttenuation = light2QuadraticAttenuation;
		
		// Kiszámoljuk az új megvilágítást, és hozzáadjuk a korábbi színhez
		vec3 shadedColor2 = lighting(light2, worldPosition, normal, material);
		shadedColor += shadedColor2;
	}

	// Textúra koordináták kiválasztása
	vec2 finalTexCoords = textureCoords;
	if (isWorldCoordsTex == 1) {
		finalTexCoords = vec2(worldPosition.x / 150.0, worldPosition.z / 150.0);
		if (ObjectType == 2) {
			finalTexCoords = vec2((worldPosition.x + ETimeInSec) / 150.0, (worldPosition.z + ETimeInSec) / 150.0);
			outputColor = texture(textureImage, finalTexCoords);
			return;
		}
	}
	
	outputColor = vec4(shadedColor, 1.0) * texture(textureImage, finalTexCoords);

	// normal vector debug:
	// outputColor = vec4( normal * 0.5 + 0.5, 1.0 );
}