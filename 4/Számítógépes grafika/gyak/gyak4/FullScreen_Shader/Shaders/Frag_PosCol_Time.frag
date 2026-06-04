#version 430

// pipeline-ból bejövő per-fragment attribútumok 
in vec2 pos;
in vec4 color;

// kimenő érték - a fragment színe 
out vec4 outputColor;

// !!!!! VARÁZSLAT !!!!
// Erről bővebben később...
uniform vec2  u_Center = vec2(0.0, 0.0);
uniform float u_Radius = 0.5;
uniform float u_Thickness = 0.05;
// !!!!!!!!!!!!

void main()
{
    float dx = pos.x - u_Center.x;
    float dy = pos.y - u_Center.y;
    float dist = sqrt(pow(dx, 2.0) + pow(dy, 2.0));

    // Meghatározzuk a körvonal határait
    float halfThickness = u_Thickness / 2.0;
    
    if (dist >= (u_Radius - halfThickness) && dist <= (u_Radius + halfThickness))
    {
        outputColor = color;
    }
    else
    {
        discard; // Ha kívül esik a körvonalon, nem rajzolunk semmit
    }
}