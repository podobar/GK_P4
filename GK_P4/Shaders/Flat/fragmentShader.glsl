 #version 450 core
 #define LIGHT_COUNT 2

in vec2 ret_textureCoords;
flat in vec3 surfaceNormal;
flat in vec3 toLightVectors[LIGHT_COUNT];
flat in vec3 toCameraVector;
flat in float visibility;

out vec4 out_Color;

uniform sampler2D textureSampler;
uniform vec3 lightColours[LIGHT_COUNT];
uniform vec3 attenuations[LIGHT_COUNT];
uniform vec3 coneOfLightDirections[LIGHT_COUNT];
uniform float coneOfLightAngles[LIGHT_COUNT];
uniform float shineDamper;
uniform float reflectivity;
uniform vec3 fogColour;

void main(void)
{
	vec3 normalizedNormalVector = normalize(surfaceNormal);
	vec3 normalizedToCameraVector = normalize(toCameraVector);

	vec3 specularSum = vec3(0.0,0.0,0.0);
	vec3 diffuseSum = vec3(0.0,0.0,0.0);
	for (int i=0; i < LIGHT_COUNT; ++i)
	{
		float distance = length(toLightVectors[i]);

		vec3 normalizedToLightVector = normalize(toLightVectors[i]);
		float intensity = 1.0 / (1.0 + (attenuations[i].x + attenuations[i].y + attenuations[i].z) * distance);

		float lightSurfaceAngle = degrees(acos(dot(-normalizedToLightVector, normalize(coneOfLightDirections[i]))));
		if(lightSurfaceAngle > coneOfLightAngles[i])
		{
			intensity = 0.0;
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
	out_Color = vec4(diffuseSum, 1.0) * texture(textureSampler, ret_textureCoords) + vec4(specularSum, 1.0);
	out_Color = mix(vec4(fogColour,1.0), out_Color, visibility);
}