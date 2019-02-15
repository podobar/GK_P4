#version 450 core
#define LIGHTS_COUNT 2

in vec3 position;
in vec2 textureCoords;
in vec3 normal;

out vec2 ret_textureCoords;
flat out vec3 surfaceNormal;
flat out vec3 toLightVector;
flat out vec3 toCameraVector;
flat out float visibility;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

uniform vec3 lightPosition;

const float density = 0.007;
const float gradient = 1.5;

void main(void){
	vec4 worldPosition = transformationMatrix * vec4(position, 1.0);
	vec4 positionRelativeToCamera = viewMatrix*worldPosition;
	gl_Position = projectionMatrix * positionRelativeToCamera;
	ret_textureCoords=textureCoords;

	surfaceNormal = (transformationMatrix * vec4(normal,0.0)).xyz;
	toLightVector = lightPosition - worldPosition.xyz;
	toCameraVector = (inverse(viewMatrix) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - worldPosition.xyz;

	float distance = length(positionRelativeToCamera.xyz);
	visibility=exp(-pow((distance*density),gradient));
}