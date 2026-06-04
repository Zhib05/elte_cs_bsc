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

// fényforrás tulajdonságok 
uniform vec4 lightPosition = vec4( 0.0, 1.0, 0.0, 1.0);

uniform vec3 La = vec3(0.2, 0.2, 0.2 );
uniform vec3 Ld = vec3(1.0, 1.0, 1.0 );
uniform vec3 Ls = vec3(1.0, 1.0, 1.0 );

uniform float lightConstantAttenuation    = 1.0;
uniform float lightLinearAttenuation      = 0.0;
uniform float lightQuadraticAttenuation   = 0.0;

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
	vec3 toLight;
	float attenuation = 1.0;

	if (light.pos.w == 0.0) //Directional
	{
		toLight = light.pos.xyz;
	} else if (light.pos.w == 1.0)
	{
		toLight = light.pos.xyz - position;
		float lightDistance = length(toLight);
		toLight /= lightDistance; // Normalize toLight

		attenuation = 1.0 / (
			light.constantAttenuation +
			light.linearAttenuation * lightDistance +
			light.quadraticAttenuation * lightDistance * lightDistance);
	}

	vec3 ambient = light.La * material.Ka;

	float diffuseFactor = max(dot(toLight, normal), 0.0) * attenuation;
	vec3 diffuse = light.Ld * material.Kd * diffuseFactor;

	vec3 toCamera = normalize(cameraPosition - position);
	vec3 reflected = reflect(-toLight, normal);
	float specularFactor = pow(max(dot(reflected, toCamera), 0.0), material.Shininess) * attenuation;
	vec3 specular = light.Ls * material.Ks * specularFactor;

	return ambient + diffuse + specular;
}

void main()
{
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

	vec3 normal = normalize(worldNormal);
	vec3 shadedColor = lighting(light, worldPosition, normal, material);
	outputColor = vec4(shadedColor, 1) * texture(textureImage, textureCoords);
}