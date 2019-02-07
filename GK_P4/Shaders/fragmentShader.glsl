 #version 450 core

in vec2 ret_textureCoords;
in vec3 surfaceNormal;
in vec3 toLightVector;
in vec3 toCameraVector;

out vec4 out_Color;

uniform sampler2D textureSampler;

uniform vec3 lightColour;

uniform float shineDamper;
uniform float reflectivity;

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

}