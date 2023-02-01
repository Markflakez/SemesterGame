using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSoundPlayer : MonoBehaviour
{
    public Tilemap tilemap;

    [Header("Grass")]
    public string grassTileName;
    public string grassTileName2;
    public string grassTileName3;
    public string grassTileName4;
    private AudioClip[] grassSounds;
    [Header("Dirt")]
    public string dirtTileName;
    public string dirtTileName2;
    public string dirtTileName3;
    public string dirtTileName4;
    public string dirtTileName5;
    private AudioClip[] dirtSounds;
    [Header("Stone")]
    public string stoneTileName;
    public string stoneTileName2;
    public string stoneTileName3;
    private AudioClip[] stoneSounds;
    public float minSpeed = 0.5f;
    public float maxSpeed = 2f;
    private AudioSource audioSource;
    private Vector3Int previousTilePos;
    private Rigidbody2D rb2d;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        previousTilePos = tilemap.WorldToCell(transform.position);
        
        grassSounds = Resources.LoadAll<AudioClip>("Footsteps - Essentials/Footsteps_Grass/Footsteps_Grass_Walk");
        dirtSounds = Resources.LoadAll<AudioClip>("Footsteps - Essentials/Footsteps_DirtyGround/Footsteps_DirtyGround_Walk");
        stoneSounds = Resources.LoadAll<AudioClip>("Footsteps - Essentials/Footsteps_Rock/Footsteps_Rock_Walk");
    }

    void Update()
    {
        Vector3Int currentTilePos = tilemap.WorldToCell(transform.position);
        //Checks if current tile is different from previous tile
        if (currentTilePos != previousTilePos)
        {
            //Gets the current tile
            TileBase currentTile = tilemap.GetTile(currentTilePos);

            //Checks if there's a tile in the current position
            if (currentTile != null)
            {
                //Checks if the current tile is a grass tile.
                if (currentTile.name == grassTileName || currentTile.name == grassTileName2 || currentTile.name == grassTileName3 || currentTile.name == grassTileName4)
                {
                    //Plays a random grass sound.
                    PlayRandomSound(grassSounds);
                }
                //Checks if the current tile is a dirt tile.
                else if (currentTile.name == dirtTileName || currentTile.name == dirtTileName2 || currentTile.name == dirtTileName3 || currentTile.name == dirtTileName4 || currentTile.name == dirtTileName5)
                {
                    //Plays a random dirt sound.
                    PlayRandomSound(dirtSounds);
                }
                //Checks if the current tile is a stone tile.
                else if (currentTile.name == stoneTileName || currentTile.name == stoneTileName2 || currentTile.name == stoneTileName3)
                {
                    //Plays a random stone sound.
                    PlayRandomSound(stoneSounds);
                }
            }
            //Updates previous tile position
            previousTilePos = currentTilePos;
        }
    }

    void PlayRandomSound(AudioClip[] sounds)
    {
        //Calculates the volume based on the speed.
        float speed = rb2d.velocity.magnitude;
        //Plays a random sound from the array with calculated volume.
        float volume = Mathf.InverseLerp(minSpeed, maxSpeed, speed) * .4f;
        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)], volume);
    }
}