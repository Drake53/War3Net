#version 450

layout(location = 0) in vec2 UV;
layout(location = 1) in vec3 Normal;
layout(location = 2) in vec3 Position;

layout(location = 0) out vec2 fsin_UV;
layout(location = 1) out vec4 fsin_Normal;

layout(binding = 0) uniform Transform { mat4 f_Transform; };
 
void main()
{
	fsin_UV = UV;
	fsin_Normal = f_Transform * vec4(Normal, 0);
	gl_Position = f_Transform * vec4(Position, 1);
}