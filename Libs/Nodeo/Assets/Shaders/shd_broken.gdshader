shader_type canvas_item;

uniform float shake_power = 0.03;
uniform float shake_rate : hint_range(0.0, 1.0) = 0.2;
uniform float shake_speed = 5.0;
uniform float shake_block_size : hint_range(0.01, 100.0) = 5.0;
uniform float shake_color_rate : hint_range(0.0, 1.0) = 0.01;
uniform float block_glitch_size : hint_range(0.01, 1.0) = 0.2;
uniform float block_glitch_rate : hint_range(0.0, 1.0) = 0.1;

uniform sampler2D screen_texture : hint_screen_texture;

float random(float seed)
{
    return fract(543.2543 * sin(dot(vec2(seed, seed), vec2(3525.46, -54.3415))));
}

void fragment()
{
    float enable_shift = float(random(trunc(TIME * shake_speed)) < shake_rate);

    vec2 fixed_uv = SCREEN_UV;
    fixed_uv.x += (
        random(
            (trunc(SCREEN_UV.y * shake_block_size) / shake_block_size) + TIME
        ) - 0.5
    ) * shake_power * enable_shift;

    float block_x = trunc(SCREEN_UV.x / block_glitch_size);
    float block_y = trunc(SCREEN_UV.y / block_glitch_size);
    float block_seed = block_x + block_y * 1000.0;
    float block_glitch_enable = random(block_seed + trunc(TIME * shake_speed)) < block_glitch_rate
    ? 1.0 : 0.0;

    if (block_glitch_enable > 0.0)
    {
        vec2 block_offset = vec2(
            (random(block_seed + 1.0) - 0.5) * shake_power,
            (random(block_seed + 2.0) - 0.5) * shake_power
        );
        fixed_uv += block_offset;
    }

    vec4 pixel_color = textureLod(screen_texture, fixed_uv, 0.0);

    pixel_color.r = mix(pixel_color.r, textureLod(screen_texture, fixed_uv + vec2(shake_color_rate, 0.0), 0.0).r, enable_shift);
    pixel_color.b = mix(pixel_color.b, textureLod(screen_texture, fixed_uv + vec2(-shake_color_rate, 0.0), 0.0).b, enable_shift);

    COLOR = pixel_color;
}
