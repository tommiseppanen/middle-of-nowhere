Shader "Custom/BlurredOutline"
{
    Properties
    {
        _MainTex("Main Texture",2D)="black"{}
        _SceneTex("Scene Texture",2D)="black"{}
    }
    CGINCLUDE
    #pragma vertex vertexShader
    #pragma fragment fragmentShader
    #include "UnityCG.cginc"

    static const int iterations = 20;
    static const half4 color = half4(0.129, 0.588, 0.953, 1);
    static const float intensityMultiplier = 3.0;
    sampler2D _MainTex;

    struct vertexToFragment
    {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
    };

    vertexToFragment vertexShader(appdata_base input)
    {
        vertexToFragment output;
        output.vertex = mul(UNITY_MATRIX_MVP, input.vertex);
        output.uv = output.vertex.xy / 2 + 0.5;
        return output;
    }

    float calculateColorIntesity(vertexToFragment info, sampler2D source, float stepSizeX, float stepSizeY)
    {
        float colorIntensity = 0;
        for (int k = 0; k<iterations; k += 1)
        {
            float offset = (k - iterations / 2);
            colorIntensity += tex2D(source, info.uv.xy + float2(offset*stepSizeX, offset*stepSizeY)).r / iterations;
        }
        return colorIntensity;
    }
    ENDCG
    SubShader 
    {
        Pass 
        {
            CGPROGRAM
            float2 _MainTex_TexelSize;  
            half fragmentShader(vertexToFragment input) : COLOR
            {
                return calculateColorIntesity(input, _MainTex, _MainTex_TexelSize.x, 0);
            }          
            ENDCG
        }   
         
        GrabPass{}
         
        Pass 
        {
            CGPROGRAM
            sampler2D _SceneTex;
            sampler2D _GrabTexture;
            float2 _GrabTexture_TexelSize;           
            half4 fragmentShader(vertexToFragment input) : COLOR
            {
                float2 coordinates = float2(input.uv.x, 1 - input.uv.y);
                //Do not render anything on top of scene texture if there is something underneath in the source texture
                if(tex2D(_MainTex, coordinates).r>0)
                {
                    return tex2D(_SceneTex, coordinates);
                }
                float colorIntensity = calculateColorIntesity(input, _GrabTexture, 0, _GrabTexture_TexelSize.y);
                return colorIntensity*color*intensityMultiplier+(1-colorIntensity)*tex2D(_SceneTex, coordinates);
            }         
            ENDCG
        }  
    }
}