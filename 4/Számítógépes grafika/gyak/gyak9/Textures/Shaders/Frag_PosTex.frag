#version 430

// pipeline-ból bejövő per-fragment attribútumok 
in vec2 textureCoords;

// kimenő érték - a fragment színe 
out vec4 outputColor;
out vec4 outputColor2;

// textúra mintavételező objektum 
uniform sampler2D textureImage;
uniform sampler2D textureMask;

void main()
{
	float mask = texture( textureMask, textureCoords ).r;
	if (mask < 0.5)
		discard;

	outputColor = texture( textureImage, textureCoords );
}