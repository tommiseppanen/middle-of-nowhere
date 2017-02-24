Shader "Custom/White"
{
    SubShader 
    {
        ZWrite Off
        ZTest Always
        Lighting Off
        Pass
        {
			CGPROGRAM
            #pragma vertex vertexShader
            #pragma fragment fragmentShader
 
            struct vertexToFragment
            {
                float4 vertex:SV_POSITION;
            };
 
			vertexToFragment vertexShader(vertexToFragment input)
            {
				vertexToFragment output;
				output.vertex = mul(UNITY_MATRIX_MVP, input.vertex);
                return output;
            }
 
            half4 fragmentShader():COLOR0
            {
                return half4(1,1,1,1);
            }
 
            ENDCG
        }
    }
}