using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSoundPlayer : MonoBehaviour
{
    public Tilemap tilemap;

    [Header("Grass")]
    public string grassTileName;
    private AudioClip[] grassSounds;
    [Header("Dirt")]
    public string dirtTileName;
    private AudioClip[] dirtSounds;
    [Header("Stone")]
    public string stoneTileName;
    private AudioClip[] stoneSounds;
    public float minSpeed = 0.5f;
    public float maxSpeed = 2f;
    private AudioSource audioSource;
    private Vector3Int previousTilePos;
    private Rigidbody2D rb2d;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        previousTilePos = tilemap.WorldToCell(transform.position);
        rb2d = GetComponent<Rigidbody2D>();
        
        grassSounds = Resources.LoadAll<AudioClip>("Footsteps - Essentials/Footsteps_Grass/Footsteps_Grass_Walk");
        dirtSounds = Resources.LoadAll<AudioClip>("Footsteps - Essentials/Footsteps_DirtyGround/Footsteps_DirtyGround_Walk");
        stoneSounds = Resources.LoadAll<AudioClip>("Footsteps - Essentials/Footsteps_Rock/Footsteps_Rock_Walk");
    }

    void Update()
    {
        Vector3Int currentTilePos = tilemap.WorldToCell(transform.position);
        if (currentTilePos != previousTilePos)
        {
            TileBase currentTile = tilemap.GetTile(currentTilePos);
            if (currentTile != null)
            {
                if (currentTile.name == grassTileName)
                {
                    PlayRandomSound(grassSounds);
                }
                else if (currentTile.name == dirtTileName)
                {
                    PlayRandomSound(dirtSounds);
                }
                else if (currentTile.name == stoneTileName)
                {
                    PlayRandomSound(stoneSounds);
                }
            }
            previousTilePos = currentTilePos;
        }
    }

    void PlayRandomSound(AudioClip[] sounds)
    {
        float speed = rb2d.velocity.magnitude;
        float volume = Mathf.InverseLerp(minSpeed, maxSpeed, speed) * 0.2f;
        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)], volume);
    }
}