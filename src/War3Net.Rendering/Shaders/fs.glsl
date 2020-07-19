#version 450

layout(location = 0) in vec2 fsin_UV;
layout(location = 1) in vec4 fsin_Normal;

layout(location = 0) out vec4 fsout_Color;

layout(binding = 0) uniform sampler2D Pixels;

void main()
{
    fsout_Color = texture(Pixels, fsin_UV);
}