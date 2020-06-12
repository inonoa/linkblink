#ifndef EXAMPLE_INCLUDED
#define EXAMPLE_INCLUDED

float3 rgb2hsv(float3 rgb)
{
    float3 hsv;

    float maxValue = rgb.r;
    int maxIdx = 0;
    {
        maxIdx = rgb.g > maxValue ? 1 : maxIdx;
        maxValue = rgb.g > maxValue ? rgb.g : maxValue;
        maxIdx = rgb.b > maxValue ? 2 : maxIdx;
        maxValue = rgb.b > maxValue ? rgb.b : maxValue;
    }
    float minValue = rgb.r;
    int minIdx = 0;
    {
        minIdx = rgb.g < minValue ? 1 : minIdx;
        minValue = rgb.g < minValue ? rgb.g : minValue;
        minIdx = rgb.b < minValue ? 2 : minIdx;
        minValue = rgb.b < minValue ? rgb.b : minValue;
    }
    
    float delta = maxValue - minValue;
            
    // V（明度）
    // 一番強い色をV値にする
    hsv.z = maxValue;
            
    // S（彩度）
    // 最大値と最小値の差を正規化して求める
    if(maxValue != 0.0){
        hsv.y = delta / maxValue;
    }else{
        hsv.y = 0.0;
    }
            
    // H（色相）
    // RGBのうち最大値と最小値の差から求める
    if (hsv.y > 0.0){
        float delta_rev = ((delta == 0.0) ? 0.0 : 1.0 / delta);

        if(maxIdx == 0){        hsv.x =     (rgb.g - rgb.b) * delta_rev;
        }else if (maxIdx == 1){ hsv.x = 2.0 + (rgb.b - rgb.r) * delta_rev;
        }else{                  hsv.x = 4.0 + (rgb.r - rgb.g) * delta_rev; }
        if(hsv.x < 0.0){ hsv.x += 6.0; }
        hsv.x *= 1.0/6.0;
    }

    return saturate(hsv);
}

float3 hsv2rgb(float3 hsv_)
{
    float3 hsv = hsv_;
    float3 rgb;

    if(hsv.y == 0){
        rgb.r = rgb.g = rgb.b = hsv.z;
        return rgb;
    }

    hsv.x *= 6.0;
    float i = floor (hsv.x);
    float f = hsv.x - i;
    float aa = hsv.z * (1 - hsv.y);
    float bb = hsv.z * (1 - (hsv.y * f));
    float cc = hsv.z * (1 - (hsv.y * (1 - f)));

    if( i < 0.5 ){       rgb.r = hsv.z; rgb.g = cc;    rgb.b = aa;
    }else if( i < 1.5 ){ rgb.r = bb;    rgb.g = hsv.z; rgb.b = aa;
    }else if( i < 2.5 ){ rgb.r = aa;    rgb.g = hsv.z; rgb.b = cc;
    }else if( i < 3.5 ){ rgb.r = aa;    rgb.g = bb;    rgb.b = hsv.z;
    }else if( i < 4.5 ){ rgb.r = cc;    rgb.g = aa;    rgb.b = hsv.z;
    }else{               rgb.r = hsv.z; rgb.g = aa;    rgb.b = bb;
    }

    return rgb;
}

#endif