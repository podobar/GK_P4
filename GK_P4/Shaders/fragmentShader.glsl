 #version 450 core

in vec2 ret_textureCoords;

out vec4 out_Color;

uniform sampler2D textureSampler;

void main(void){
	out_Color = texture(textureSampler, ret_textureCoords);

}