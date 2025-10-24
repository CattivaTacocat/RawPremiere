shader_type canvas_item;

// 进度控制参数
uniform float progress : hint_range(0.0, 1.0) = 0.0;

// 透明度控制参数
uniform float alpha : hint_range(0.0, 1.0) = 1.0;

// 黑屏控制参数
uniform bool black_screen = false;
uniform vec4 cover_color : source_color = vec4(0,0,0.5,1);

// 故障效果参数
uniform float intensity : hint_range(0.0, 1.0) = 0.5;
uniform float glitch_frequency : hint_range(0.1, 10.0) = 2.0;
uniform float glitch_duration : hint_range(0.01, 0.5) = 0.1;

uniform float red_displacement : hint_range(0.0, 0.1) = 0.02;
uniform float green_displacement : hint_range(0.0, 0.1) = 0.02;
uniform float blue_displacement : hint_range(0.0, 0.1) = 0.02;

uniform float scanline_intensity : hint_range(0.0, 1.0) = 0.3;
uniform float noise_intensity : hint_range(0.0, 1.0) = 0.2;
uniform float slice_intensity : hint_range(0.0, 1.0) = 0.4;
uniform float ghost_intensity : hint_range(0.0, 0.5) = 0.1;
uniform float negative_intensity : hint_range(0.0, 1.0) = 0.0;

// 震动效果参数
uniform float shake_power : hint_range(0.0, 0.1) = 0.03;
uniform float shake_rate : hint_range(0.0, 1.0) = 0.2;
uniform float shake_speed : hint_range(0.1, 20.0) = 5.0;
uniform float shake_block_size : hint_range(1.0, 100.0) = 30.5;
uniform float shake_color_rate : hint_range(0.0, 0.1) = 0.01;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;

// 随机函数
float random(vec2 st) {
    return fract(sin(dot(st.xy, vec2(12.9898, 78.233))) * 43758.5453123);
}

float random_seed(float seed) {
    return fract(543.2543 * sin(dot(vec2(seed, seed), vec2(3525.46, -54.3415))));
}

float noise(vec2 st) {
    vec2 i = floor(st);
    vec2 f = fract(st);

    float a = random(i);
    float b = random(i + vec2(1.0, 0.0));
    float c = random(i + vec2(0.0, 1.0));
    float d = random(i + vec2(1.0, 1.0));

    vec2 u = f * f * (3.0 - 2.0 * f);

    return mix(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

void fragment() {
    // 黑屏控制 - 使用条件判断而非 return
    if (black_screen) {
        COLOR = cover_color;
    } else {
        vec2 uv = SCREEN_UV;
        vec4 base_color = texture(SCREEN_TEXTURE, uv);
        vec4 final_color = base_color;

        // 使用 progress 控制整体效果强度
        float effective_intensity = intensity * progress;
        float effective_shake_power = shake_power * progress;
        float effective_shake_rate = shake_rate * progress;

        // 应用震动效果
        float enable_shift = float(random_seed(trunc(TIME * shake_speed)) < effective_shake_rate);

        vec2 fixed_uv = uv;
        fixed_uv.x += (random_seed((trunc(uv.y * shake_block_size) / shake_block_size) + TIME) - 0.5)
                      * effective_shake_power * enable_shift;

        // 使用震动后的UV作为故障效果的输入
        uv = fixed_uv;

        // 故障效果时间因子
        float time_factor = TIME * glitch_frequency * progress;
        float glitch_noise = noise(vec2(time_factor, 0.0));

        // 使用 progress 控制故障激活概率
        float glitch_active = step(1.0 - effective_intensity, glitch_noise) * progress;

        // 只有当 progress > 0 时才应用故障效果
        if (progress > 0.0 && glitch_active > 0.5) {
            float slice_y = random(vec2(time_factor * 10.0, 0.0));
            if (abs(uv.y - slice_y) < 0.02 * slice_intensity * progress) {
                float slice_offset = (random(vec2(time_factor * 15.0, 0.0)) - 0.5) * 0.1 * slice_intensity * progress;
                uv.x += slice_offset;
            }

            float vertical_shift = random(vec2(time_factor * 8.0, 0.0)) * 0.05 * effective_intensity;
            uv.y += vertical_shift;

            float wave = sin(uv.y * 100.0 + time_factor * 5.0) * 0.01 * effective_intensity;
            uv.x += wave;

            final_color = texture(TEXTURE, uv);
        }

        // 使用 progress 控制颜色偏移强度
        float shift_amount = red_displacement * glitch_active * progress;
        vec4 red_channel = texture(SCREEN_TEXTURE, uv + vec2(shift_amount, 0.0));
        vec4 green_channel = texture(SCREEN_TEXTURE, uv + vec2(-green_displacement * glitch_active * progress, 0.0));
        vec4 blue_channel = texture(SCREEN_TEXTURE, uv + vec2(blue_displacement * glitch_active * progress, 0.0));

        final_color.r = mix(final_color.r, red_channel.r, glitch_active);
        final_color.g = mix(final_color.g, green_channel.g, glitch_active);
        final_color.b = mix(final_color.b, blue_channel.b, glitch_active);

        // 使用 progress 控制扫描线强度
        float scanline = sin(uv.y * 300.0 + time_factor * 2.0) * 0.5 + 0.5;
        final_color.rgb *= 1.0 - scanline * scanline_intensity * effective_intensity;

        // 使用 progress 控制静态噪声强度
        float static_noise = random(uv + time_factor) * noise_intensity * glitch_active * progress;
        final_color.rgb += static_noise;

        // 使用 progress 控制重影效果
        if (ghost_intensity > 0.0) {
            vec4 ghost_color = texture(SCREEN_TEXTURE, uv + vec2(0.01, 0.01)) * ghost_intensity * progress;
            final_color.rgb = mix(final_color.rgb, ghost_color.rgb, ghost_intensity * 0.5 * progress);
        }

        // 使用 progress 控制负片效果
        if (negative_intensity > 0.0) {
            vec4 negative_color = vec4(1.0 - final_color.rgb, final_color.a);
            final_color.rgb = mix(final_color.rgb, negative_color.rgb, negative_intensity * glitch_active * progress);
        }

        // 使用 progress 控制发光效果
        float glow = glitch_active * 0.2 * effective_intensity;
        final_color.rgb += glow;

        // 应用震动效果的颜色分离
        if (enable_shift > 0.5) {
            final_color.r = mix(
                final_color.r,
                texture(TEXTURE, uv + vec2(shake_color_rate * progress, 0.0)).r,
                enable_shift
            );
            final_color.b = mix(
                final_color.b,
                texture(TEXTURE, uv + vec2(-shake_color_rate * progress, 0.0)).b,
                enable_shift
            );
        }

        // 应用透明度控制
        final_color.a *= alpha;

        COLOR = final_color;
    }
}