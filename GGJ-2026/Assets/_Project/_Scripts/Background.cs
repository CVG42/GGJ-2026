using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GGJ
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private Vector2 velMove;

        private Vector2 offset;
        private Material material;
        private Rigidbody playerRB;

        private void Awake()
        {
            material =  GetComponent<SpriteRenderer>().material;

            playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        }

        private void Update()
        {
            offset = (playerRB.velocity.x * 0.1f) * velMove * Time.deltaTime;

            material.mainTextureOffset += offset;
        }

    }
}
