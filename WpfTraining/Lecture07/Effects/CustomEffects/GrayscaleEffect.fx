/// <class>GrayscaleEffect</class>
/// <description>An effect that turns the input into shades of gray.</description>

sampler2D  inputSampler : register(S0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 sourceColor = tex2D(inputSampler, uv); // Get the source color
    float3 rgb = sourceColor.rgb; // Get RGB components of the source color
    float3 resultColor = dot(rgb, float3(0.2126, 0.7152, 0.0722)); // Get the result RGB components.
    return float4(resultColor, sourceColor.a); // Return the result (with the unchanged A channel).
}