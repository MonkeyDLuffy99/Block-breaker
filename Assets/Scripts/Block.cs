using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprites;
    Level level;

    [SerializeField] int timesHit;

    void Start() {
        CountBlocks();
    }

    private void CountBlocks() {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable") {
            level.CountBreakableBlocks();
        } 
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (tag == "Breakable") {
            HandleHit();
        }  
    }

    private void HandleHit() {
        timesHit ++;
        int maxHits = hitSprites.Length+1;
        if (timesHit >= maxHits) {
            DestroyBlock();
        }
        else {
            ShowNextHitSprite();
        } 
    }

    private void ShowNextHitSprite() {
        int spriteIndex = timesHit-1;
        if (hitSprites[spriteIndex] != null) {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else {
            Debug.LogError("Block sprite is missing from array" + gameObject.name);
        }
    }

    private void DestroyBlock() {
        PlayBlockDestroySFX();
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerSparklesVFX();
    }

    private void TriggerSparklesVFX() {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }

    private void PlayBlockDestroySFX() {
        FindObjectOfType<GameStatus>().AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }
}
