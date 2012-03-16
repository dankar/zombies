varying vec4 diffuse,ambientGlobal,ambient;
varying vec3 normal,lightDir,halfVector;
varying float dist;

uniform sampler2D texture;

vec4 Lighting()
{
	vec3 n,halfV,viewV,ldir;
	float NdotL,NdotHV;
	
	float att;
	
	vec4 color = ambientGlobal;
	
	n = normalize(normal);
	
	NdotL = max(dot(n,normalize(lightDir)),0.0);
	
	if(NdotL > 0.0)
	{
		att = 1.0 / (gl_LightSource[0].constantAttenuation + gl_LightSource[0].linearAttenuation * dist + gl_LightSource[0].quadraticAttenuation * dist * dist);
		color += att * (diffuse * NdotL + ambient);
		
		halfV = normalize(halfVector);
		NdotHV = max(dot(n,halfV),0.0);
		
		color += att * gl_FrontMaterial.specular * gl_LightSource[0].specular * pow(NdotHV,gl_FrontMaterial.shininess);
		
	}
	
	return color;
}

void main()
{
	vec4 color = texture2D(texture,gl_TexCoord[0].st);
	vec4 light = Lighting();

	color.x *= light.x;
	color.y *= light.y;
	color.z *= light.z;
	gl_FragColor = color;
}