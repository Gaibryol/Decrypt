using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Letter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public string Character;

	[SerializeField, Header("Images")] private Image image;
	[SerializeField] private Sprite incorrectImage;
	[SerializeField] private Sprite correctImage;

	private RectTransform rectTransform;
	private Word word;
	private bool isDown;

	private int originalIndex;
	private float originalX;

	private void Awake()
	{
		word = transform.parent.GetComponent<Word>();
		rectTransform = GetComponent<RectTransform>();
		isDown = true;
	}

	private void Update()
	{
		if (!isDown)
		{
			transform.position = new Vector3(Input.mousePosition.x, transform.position.y, 0);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		originalIndex = word.GetIndex(this.gameObject);
		originalX = rectTransform.position.x;
		transform.SetParent(GameManager.Instance.Canvas.transform);
		isDown = false;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		rectTransform.SetParent(word.transform);

		float diff = Mathf.Abs(originalX - eventData.position.x);
		int movement = 0;
		if (diff > 0)
		{
			// Went left
			movement = Mathf.RoundToInt(Mathf.Abs(originalX - eventData.position.x - (rectTransform.rect.width / 2)) / rectTransform.rect.width);
		}
		else if (diff < 0)
		{
			// Went right
			movement = Mathf.RoundToInt(Mathf.Abs(originalX - eventData.position.x + (rectTransform.rect.width / 2)) / rectTransform.rect.width);
		}

		if (originalX - eventData.position.x > 0)
		{
			// Went left
			if (originalIndex - movement <= 0)
			{
				rectTransform.SetSiblingIndex(0);
			}
			else
			{
				rectTransform.SetSiblingIndex(originalIndex - movement);
			}
		}
		else if (originalX - eventData.position.x < 0)
		{
			// Went right
			if (originalIndex + movement >= rectTransform.parent.childCount - 1)
			{
				rectTransform.SetSiblingIndex(rectTransform.parent.childCount - 1);
			}
			else
			{
				rectTransform.SetSiblingIndex(originalIndex + movement);
			}
		}
		else
		{
			rectTransform.SetSiblingIndex(originalIndex);
		}
		isDown = true;
	}
}