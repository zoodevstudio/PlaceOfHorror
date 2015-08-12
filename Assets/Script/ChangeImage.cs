using UnityEngine;
using System.Collections;

public class ChangeImage : MonoBehaviour
{
	public bool justOneTime;
	private bool currentSprite;
	public Sprite DefaultSprite;
	public Sprite ChangeSprite;
	private GameObject Player;
	void Start ()
	{
		Player = GameObject.FindGameObjectWithTag("Player");
		if (!currentSprite)
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = DefaultSprite;
		else
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = ChangeSprite;
	}

	public void Change ()
	{
		if (!justOneTime) {
			if (!currentSprite) {
				currentSprite = true;
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = ChangeSprite;
			} else {
				currentSprite = false;
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = DefaultSprite;
			}
		} else {
			if (this.gameObject.GetComponent<BoxCollider2D> () != null) {
				this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
				Player.GetComponent<ItemPriority>().RemoveAtList(4);
			}
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = ChangeSprite;
			//this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			Transform _canvas = GameObject.FindGameObjectWithTag ("Canvas").transform;
			Transform talkBubble = _canvas.FindChild ("TalkBubble");
			talkBubble.gameObject.SetActive (false);
			this.gameObject.GetComponent<ChangeImage> ().enabled = false;
		}
	}
}
