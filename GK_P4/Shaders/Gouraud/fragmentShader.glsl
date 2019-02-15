 #version 450 core
 #define LIGHTS_COUNT 2

in vec2 ret_textureCoords;
flat in vec3 surfaceNormal;
flat in vec3 toLightVector;
flat in vec3 toCameraVector;
flat in float visibility;

out vec4 out_Color;

uniform sampler2D textureSampler;
uniform vec3 lightColour;
uniform float shineDamper;
uniform float reflectivity;
uniform vec3 fogColour;

void main(void){
	vec3 unitNormalVector = normalize(surfaceNormal);
	vec3 unitLightVector = normalize(toLightVector);

	float prod = dot(unitNormalVector,unitLightVector);
	float brightness = max(prod, 0.2);
	vec3 diffuse = brightness * lightColour;

	vec3 unitToCameraVector = normalize(toCameraVector);
	vec3 lightDirection = -unitLightVector;
	vec3 reflectedLightDirection = reflect(lightDirection, unitNormalVector);

	float specularFactor = dot(reflectedLightDirection, unitToCameraVector);
	specularFactor = max(specularFactor, 0.0);
	float dampedFactor = pow(specularFactor, shineDamper);
	vec3 finalSpecular = dampedFactor * reflectivity * lightColour;

	out_Color = vec4(diffuse, 1.0) * texture(textureSampler, ret_textureCoords) + vec4(finalSpecular, 1.0);
	out_Color = mix(vec4(fogColour,1.0), out_Color, visibility);
}