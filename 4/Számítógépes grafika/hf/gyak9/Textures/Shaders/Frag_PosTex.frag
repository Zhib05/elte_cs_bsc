#version 430

// pipeline-ból bejövő per-fragment attribútumok 
in vec2 textureCoords;
in float worldY;

// kimenő érték - a fragment színe 
out vec4 outputColor;

// textúra mintavételező objektum 
uniform sampler2D textureImage;
uniform sampler2D textureMask;
uniform sampler2D textureDirt;

uniform bool useMask; // Kapcsoló a maszkoláshoz

void main()
{
    if (useMask)
    {
        float mask = texture( textureMask, textureCoords ).r;
        if (mask < 0.5)
            discard;
    }

    vec4 baseColor = texture( textureImage, textureCoords );
    vec4 dirtColor = texture( textureDirt, textureCoords );

    float keveres = worldY / 1.5;

	if (keveres < 0.0) 
	{
		keveres = 0.0;
	} 
	else if (keveres > 1.0) 
	{
		keveres = 1.0;
	}

    outputColor = mix(dirtColor, baseColor, keveres);
}