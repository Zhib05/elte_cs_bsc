#version 430

// lokális változók: két tömb 
vec4 positions[6] = vec4[6](
	vec4(-1, -1, 0, 1),
	vec4( 1, -1, 0, 1),
	vec4(-1,  1, 0, 1),

	vec4(-1,  1, 0, 1),
	vec4( 1, -1, 0, 1),
	vec4( 1,  1, 0, 1)
);

vec4 colors[6] = vec4[6](
	vec4(1, 0, 0, 1),
	vec4(0, 1, 0, 1),
	vec4(0, 0, 1, 1),

	vec4(0, 0, 1, 1),
	vec4(0, 1, 0, 1),
	vec4(1, 1, 1, 1)
);


// a pipeline-ban tovább adandó értékek 
out vec4 color;
uniform float m_translate_X = 0.0;
uniform float m_translate_Y = 0.0;

void main()
{
	mat4 identity = mat4(
		1, 0, 0, 0,
		0, 1, 0, 0,
		0, 0, 1, 0,
		0, 0, 0, 1);

	mat4 translate = mat4(
		1, 0, 0, m_translate_X,
		0, 1, 0, m_translate_Y,
		0, 0, 1, 0,
		0, 0, 0, 1);

	float a = 3.14 / 4.0;
	float ca = cos(a);
	float sa = sin(a);
	mat4 rotateZ = mat4(
		ca, -sa, 0, 0,
		sa,  ca, 0, 0,
		0,   0, 1, 0,
		0,   0, 0, 1);

	mat4 rotateY = mat4(
		ca, 0, -sa, 0,
		0,  1, 0, 0,
		sa, 0, ca, 0,
		0,  0, 0, 1);

	mat4 scale = mat4(
		0.25, 0, 0, 0,
		0, 0.25, 0, 0,
		0, 0, 1, 0,
		0, 0, 0, 1);

	// gl_VertexID: https://www.khronos.org/registry/OpenGL-Refpages/gl4/html/gl_VertexID.xhtml
	// Általában mindig ez a sorrend a transzformációknak: scale(skálázunk) -> rotate(elforgatunk) -> translate(eltolunk)
	gl_Position  = transpose(translate) * transpose(rotateZ) * transpose(scale) * positions[gl_VertexID];
	color = colors[gl_VertexID];
}