sampler2D  backgroundSampler : register(S0);
sampler2D  foregroundSampler : register(S1);

/// <summary>The color that becomes transparent.</summary>
/// <defaultValue>#FF00FF62</defaultValue>
float4 ChromaKey : register(C0);

/// <summary>The tolerance in color differences.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float Tolerance : register(C1);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 bgColor = tex2D(backgroundSampler, uv);
    float4 fgColor = tex2D(foregroundSampler, uv);

    if(all(abs(fgColor.rgb - ChromaKey.rgb) < Tolerance))
    {
    		return bgColor;
    } 
    else
    {
    		return fgColor;
    }
}