#ifndef EXAMPLE_INCLUDED
#define EXAMPLE_INCLUDED

float3 rgb2hsv(float3 rgb)
{
    float3 hsv;

    float maxValue = max(rgb.r, max(rgb.g, rgb.b));
    float minValue = min(rgb.r, min(rgb.g, rgb.b));
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
        if(rgb.r == maxValue){        hsv.x = (rgb.g - rgb.b) / delta;
        }else if (rgb.g == maxValue){ hsv.x = 2 + (rgb.b - rgb.r) / delta;
        }else{                        hsv.x = 4 + (rgb.r - rgb.g) / delta; }
        hsv.x /= 6.0;
        if(hsv.x < 0){ hsv.x += 1.0; }
    }
            
    return hsv;
}
        
float3 hsv2rgb(float3 hsv)
{
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

    if( i < 1 ){       rgb.r = hsv.z; rgb.g = cc;    rgb.b = aa;
    }else if( i < 2 ){ rgb.r = bb;    rgb.g = hsv.z; rgb.b = aa;
    }else if( i < 3 ){ rgb.r = aa;    rgb.g = hsv.z; rgb.b = cc;
    }else if( i < 4 ){ rgb.r = aa;    rgb.g = bb;    rgb.b = hsv.z;
    }else if( i < 5 ){ rgb.r = cc;    rgb.g = aa;    rgb.b = hsv.z;
    }else{             rgb.r = hsv.z; rgb.g = aa;    rgb.b = bb;
    }

    return rgb;
}

#endif