#version 450 core
#define LIGHT_COUNT 4

in vec2 ret_textureCoords;
in float visibility;
in vec3 specularSum;
in vec3 diffuseSum;
out vec4 out_Color;

uniform sampler2D textureSampler;
uniform float shineDamper;
uniform float reflectivity;
uniform vec3 fogColour;

void main(void)
{
	
	out_Color = vec4(diffuseSum, 1.0) * texture(textureSampler, ret_textureCoords) + vec4(specularSum, 1.0);
	out_Color = mix(vec4(fogColour,1.0), out_Color, visibility);
}