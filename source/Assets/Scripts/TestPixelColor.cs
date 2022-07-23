using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPixelColor : MonoBehaviour
{
    [SerializeField]SpriteRenderer spriteRenderer;
    [SerializeField] RawImage rawImage;
    [SerializeField] Image image;
    Texture2D textureCopy;
    private void Start()
    {
        //PixelColor();
        var mainTexture = spriteRenderer.sprite.texture;
        //copy
        textureCopy = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
        Graphics.Blit(mainTexture, renderTexture);

        RenderTexture.active = renderTexture;
        textureCopy.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        textureCopy.Apply();
    }

    private void Update()
    {
        GetSpritePixelColorUnderMousePointer(spriteRenderer, out Color color);
        //image.color = color;
        //Debug.LogError((color.r + color.g + color.b ).ToString());
    }

    void PixelColor()
    {
        Sprite sprite = spriteRenderer.sprite;
        Texture2D mainTexture = sprite.texture;
        Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
        Graphics.Blit(mainTexture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        rawImage.texture = texture2D;

        //Color[] pixels = mainTexture.GetPixels();
        //for (int i = 0; i < pixels.Length; i++)
        //{
        //    Debug.LogError(i);
        //}

    }

    public bool GetSpritePixelColorUnderMousePointer(SpriteRenderer spriteRenderer, out Color color)
    {
        color = new Color();
        Camera cam = Camera.main;
        Vector2 mousePos = Input.mousePosition;
        Vector2 viewportPos = cam.ScreenToViewportPoint(mousePos);
        if (viewportPos.x < 0.0f || viewportPos.x > 1.0f || viewportPos.y < 0.0f || viewportPos.y > 1.0f) return false; // out of viewport bounds
                                                                                                                        // Cast a ray from viewport point into world
        Ray ray = cam.ViewportPointToRay(viewportPos);

        // Check for intersection with sprite and get the color
        return IntersectsSprite(spriteRenderer, ray, out color);
    }

    private bool IntersectsSprite(SpriteRenderer spriteRenderer, Ray ray, out Color color)
    {
        color = new Color();
        if (spriteRenderer == null) return false;
        Sprite sprite = spriteRenderer.sprite;
        if (sprite == null) return false;
        Texture2D mainTexture = sprite.texture;
        if (mainTexture == null) return false;
        // Check atlas packing mode
        if (sprite.packed && sprite.packingMode == SpritePackingMode.Tight)
        {
            // Cannot use textureRect on tightly packed sprites
            Debug.LogError("SpritePackingMode.Tight atlas packing is not supported!");
            // TODO: support tightly packed sprites
            return false;
        }
        // Craete a plane so it has the same orientation as the sprite transform
        Plane plane = new Plane(transform.forward, transform.position);
        // Intersect the ray and the plane
        float rayIntersectDist; // the distance from the ray origin to the intersection point
        if (!plane.Raycast(ray, out rayIntersectDist)) return false; // no intersection
                                                                     // Convert world position to sprite position
                                                                     // worldToLocalMatrix.MultiplyPoint3x4 returns a value from based on the texture dimensions (+/- half texDimension / pixelsPerUnit) )
                                                                     // 0, 0 corresponds to the center of the TEXTURE ITSELF, not the center of the trimmed sprite textureRect
        Vector3 spritePos = spriteRenderer.worldToLocalMatrix.MultiplyPoint3x4(ray.origin + (ray.direction * rayIntersectDist));
        Rect textureRect = sprite.textureRect;
        float pixelsPerUnit = sprite.pixelsPerUnit;
        float halfRealTexWidth = mainTexture.width * 0.5f; // use the real texture width here because center is based on this -- probably won't work right for atlases
        float halfRealTexHeight = mainTexture.height * 0.5f;
        // Convert to pixel position, offsetting so 0,0 is in lower left instead of center
        int texPosX = (int)(spritePos.x * pixelsPerUnit + halfRealTexWidth);
        int texPosY = (int)(spritePos.y * pixelsPerUnit + halfRealTexHeight);
        // Check if pixel is within texture
        if (texPosX < 0 || texPosX < textureRect.x || texPosX >= Mathf.FloorToInt(textureRect.xMax)) return false; // out of bounds
        if (texPosY < 0 || texPosY < textureRect.y || texPosY >= Mathf.FloorToInt(textureRect.yMax)) return false; // out of bounds
                                                                                                                   // Get pixel color
        color = mainTexture.GetPixel(texPosX, texPosY);

        textureCopy.SetPixel(texPosX, texPosY, Color.red);
        textureCopy.Apply();

        image.sprite = Sprite.Create(textureCopy, image.sprite.rect, new Vector2(0, 1));
        return true;
    }
    /*
    public Texture2D CopyTexture2D(Texture2D copiedTexture)
    {
        //Create a new Texture2D, which will be the copy.
        Texture2D texture = new Texture2D(copiedTexture.width, copiedTexture.height);
        //Choose your filtermode and wrapmode here.
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        int y = 0;
        while (y < texture.height)
        {
            int x = 0;
            while (x < texture.width)
            {
                //INSERT YOUR LOGIC HERE
                if (copiedTexture.GetPixel(x, y) == Color.green)
                {
                    //This line of code and if statement, turn Green pixels into Red pixels.
                    texture.SetPixel(x, y, Color.red);
                }
                else
                {
                    //This line of code is REQUIRED. Do NOT delete it. This is what copies the image as it was, without any change.
                    texture.SetPixel(x, y, copiedTexture.GetPixel(x, y));
                }
                ++x;
            }
            ++y;
        }
        //Name the texture, if you want.
        texture.name = (Species + Gender + "_SpriteSheet");

        //This finalizes it. If you want to edit it still, do it before you finish with .Apply(). Do NOT expect to edit the image after you have applied. It did NOT work for me to edit it after this function.
        texture.Apply();

        //Return the variable, so you have it to assign to a permanent variable and so you can use it.
        return texture;
    }

    public void UpdateCharacterTexture()
    {
        //This calls the copy texture function, and copies it. The variable characterTextures2D is a Texture2D which is now the returned newly copied Texture2D.
        characterTexture2D = CopyTexture2D(gameObject.GetComponent<SpriteRenderer>().sprite.texture);

        //Get your SpriteRenderer, get the name of the old sprite,  create a new sprite, name the sprite the old name, and then update the material. If you have multiple sprites, you will want to do this in a loop- which I will post later in another post.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        string tempName = sr.sprite.name;
        sr.sprite = Sprite.Create(characterTexture2D, sr.sprite.rect, new Vector2(0, 1));
        sr.sprite.name = tempName;

        sr.material.mainTexture = characterTexture2D;
        sr.material.shader = Shader.Find("Sprites/Transparent Unlit");

    }*/
}
