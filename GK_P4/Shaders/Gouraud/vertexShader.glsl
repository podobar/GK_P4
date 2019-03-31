#version 450 core
#define LIGHT_COUNT 4

in vec3 position;
in vec2 textureCoords;
in vec3 normal;

out vec2 ret_textureCoords;
out float visibility;
out vec3 specularSum;
out vec3 diffuseSum;

uniform vec3 lightPositions[LIGHT_COUNT];
uniform vec3 lightColours[LIGHT_COUNT];
uniform vec3 attenuations[LIGHT_COUNT];
uniform vec3 coneOfLightDirections[LIGHT_COUNT];
uniform float coneOfLightAngles[LIGHT_COUNT];
uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform float shineDamper;
uniform float reflectivity;

const float density = 0.007;
const float gradient = 1.5;

void main(void){
	specularSum = vec3(0.0,0.0,0.0);
	diffuseSum = vec3(0.0,0.0,0.0);
	vec4 worldPosition = transformationMatrix * vec4(position, 1.0);
	vec4 positionRelativeToCamera = viewMatrix*worldPosition;
	gl_Position = projectionMatrix * positionRelativeToCamera;
	ret_textureCoords=textureCoords;

	vec3 surfaceNormal = (transformationMatrix * vec4(normal,0.0)).xyz;
	vec3 toLightVectors[LIGHT_COUNT];
	for ( int i = 0; i < LIGHT_COUNT; ++i)
	{
		toLightVectors[i] = lightPositions[i] - worldPosition.xyz;
	}
	vec3 toCameraVector = (inverse(viewMatrix) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - worldPosition.xyz;

	float distance = length(positionRelativeToCamera.xyz);
	visibility = exp(-pow((distance*density),gradient));

	vec3 normalizedNormalVector = normalize(surfaceNormal);
	vec3 normalizedToCameraVector = normalize(toCameraVector);

	

	for (int i=0; i < LIGHT_COUNT; ++i)
	{
		float distance = length(toLightVectors[i]);
		vec3 normalizedToLightVector = normalize(toLightVectors[i]);
		float intensity = 1.0 / (1.0 + (attenuations[i].x + attenuations[i].y + attenuations[i].z) * distance);

		float lightSurfaceAngle = degrees(acos(dot(-normalizedToLightVector, normalize(coneOfLightDirections[i]))));
		if(lightSurfaceAngle > coneOfLightAngles[i])
		{
			continue;
		}
		float brightness = max(dot(normalizedNormalVector,normalizedToLightVector), 0.1);

		vec3 lightDirection = -normalizedToLightVector;
		vec3 reflectedLightDirection = reflect(lightDirection, normalizedNormalVector);
		float specular = max(dot(reflectedLightDirection, normalizedToCameraVector), 0.0);
		float damped = pow(specular, shineDamper);

		specularSum += intensity * ((damped * reflectivity * lightColours[i]));
		diffuseSum += intensity * (brightness * lightColours[i]);
	}
	diffuseSum = max(diffuseSum, 0.1);
}