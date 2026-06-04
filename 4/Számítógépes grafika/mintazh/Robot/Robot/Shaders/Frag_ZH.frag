#version 430

// Ezek az értékek jönnek a geometriából (a Vertex Shaderből)
in vec3 worldPosition;
in vec3 worldNormal;
in vec2 textureCoords;

// Ez lesz a pixel végleges színe
out vec4 outputColor;

// Külső paraméterek a C++ programból
uniform sampler2D textureImage;
uniform vec3 cameraPosition; // A kameránk helyzete
uniform int objectType;      // 1 = asztallap, 0 = minden más

void main()
{
    // 1. Alapszín kiolvasása a képről (textúrából)
    vec4 baseColor = texture(textureImage, textureCoords);

    // ==========================================
    // 2.2 ASZTAL MINTA (Sakktábla)
    // ==========================================
    // Csak akkor módosítjuk a színt, ha asztallapot rajzolunk (objectType == 1)
    if (objectType == 1) {
        // A feladat által megadott képlet: val = floor(8 * u) + floor(8 * v)
        int val = int(floor(8.0 * textureCoords.x) + floor(8.0 * textureCoords.y));
        
        // A "% 2" a kettes maradékos osztás. Ha nem nulla, akkor a szám páratlan.
        if (val % 2 != 0) {
            // A színösszetevőket (Red, Green, Blue) megszorozzuk 0.8-cal (sötétítjük)
            baseColor.rgb *= 0.8; 
        }
    }

    // ==========================================
    // 2.1 ALAP ÁRNYALÁS (Phong-modell)
    // ==========================================
    // N: Normálvektor (a felületre merőleges irány, megmutatja merre néz a felület)
    vec3 N = normalize(worldNormal);
    
    // L: Fényirány. A feladat szerint a fény 1/3*(1,2,2) irányból érkezik.
    vec3 L = normalize(vec3(1.0/3.0, 2.0/3.0, 2.0/3.0));
    
    // V: Kamera irány (a pontból a kamera felé mutató vektor)
    vec3 V = normalize(cameraPosition - worldPosition);

    // A) Diffúz (szórt) fény: Kiszámolja, mennyire néz a felület a fény felé.
    // A dot() a skaláris szorzat. Ha a felület a fény felé néz, az érték 1 közeli. Ha elfordul, 0.
    float kd = max(dot(N, L), 0.0);

    // B) Spekuláris (csillogó) fény
    vec3 R = reflect(-L, N); // A fény visszaverődési iránya
    float shininess = 16.0;  // Ideiglenes csillogási érték a 2.3-as feladatig
    float ks = pow(max(dot(V, R), 0.0), shininess);

    // C) Színek összekeverése
    vec3 ambient = baseColor.rgb * 0.2; // Alap megvilágítás (hogy az árnyékban lévő részek se legyenek koromsötétek)
    vec3 diffuse = baseColor.rgb * kd;  // Fény által megvilágított szín
    vec3 specular = vec3(1.0) * ks;     // Fehér csillogás

    // Ha a felület nem kap fényt (hátul van), akkor ne is csillogjon
    if (kd <= 0.0) {
        specular = vec3(0.0);
    }

    // Végleges szín beállítása: Ambient + Diffuse + Specular
    outputColor = vec4(ambient + diffuse + specular, baseColor.a);
}