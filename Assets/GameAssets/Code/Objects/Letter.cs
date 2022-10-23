using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Letter : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public string Character;

	private Image image;
	private RectTransform rectTransform;
    private Animator animator;
	private Word word;
	private bool isDown;

	private int originalIndex;
	private float originalX;

	private bool isCovered;
	private bool canMove;
	private bool specialLetter;
	private Constants.LetterColors characterColor;

	private void Awake()
	{
		image = GetComponentInChildren<Image>();
		word = transform.parent.GetComponent<Word>();
		rectTransform = GetComponent<RectTransform>();
        animator = GetComponentInChildren<Animator>();
		isDown = true;
		isCovered = false;
	}

	private void Update()
	{
		if (!isDown)
		{
			transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		}
	}

	public void Construct(string letter, bool covered, bool move, Constants.LetterColors color, bool special)
	{
		specialLetter = special;
		Character = letter;
		isCovered = covered;
		canMove = move;
		if(!canMove)
		{
			characterColor = Constants.LetterColors.Green;
		}
		else
		{
			characterColor = color;
		}

		if (!isCovered)
		{
			RevealLetter();
		}
		else
		{
			image.sprite = LettersManager.Instance.GetTileCoverSprite();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if(!word.IsInteractable) return;
		this.gameObject.name = "PickedUp";
		word.OnPickUpLetter();
		originalIndex = word.GetIndex(this.gameObject);
		originalX = rectTransform.position.x;
		transform.SetParent(GameManager.Instance.Canvas.transform);

        Image placeHolderImage = LettersManager.Instance.placeHolder.GetComponent<Image>();
        placeHolderImage.color = new Color(placeHolderImage.color.r, placeHolderImage.color.g, placeHolderImage.color.b, 1f);
        LettersManager.Instance.placeHolder.transform.SetParent(word.transform);
        LettersManager.Instance.placeHolder.transform.SetSiblingIndex(originalIndex);

        isDown = false;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if(!word.IsInteractable) return;
		this.gameObject.name = "Letter(Clone)";
		word.OnPutDownLetter();
		int endIndex = word.GetIndexByPos(rectTransform.localPosition.x);
		rectTransform.SetParent(word.transform);
		rectTransform.SetSiblingIndex(endIndex);

        Image placeHolderImage = LettersManager.Instance.placeHolder.GetComponent<Image>();
        placeHolderImage.color = new Color(placeHolderImage.color.r, placeHolderImage.color.g, placeHolderImage.color.b, 0f);
        LettersManager.Instance.placeHolder.transform.SetParent(GameManager.Instance.Canvas.transform);
        animator.Play("LetterPulsate");

        isDown = true;

		word.CheckIsCorrect();
	}

	public void OnDrag(PointerEventData eventData)
	{
        int currentIndex = word.GetIndexByPos(rectTransform.localPosition.x);
        LettersManager.Instance.placeHolder.transform.SetSiblingIndex(currentIndex);
    }

	public void RevealLetter()
	{
		if(!specialLetter)
			image.sprite = LettersManager.Instance.GetSprite(characterColor, Character);
	}
	public void ReplaceLetter(string correctCharacter)
	{
		Character = correctCharacter;
	}

	public void ChangeToCorrect()
	{
		isCovered = false;
		image.sprite = LettersManager.Instance.GetSprite(Constants.LetterColors.Green, Character);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if(HacksManager.Instance.ActivatedE & isCovered == true){
			image.sprite = LettersManager.Instance.GetSprite(characterColor, Character);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if(HacksManager.Instance.ActivatedE & isCovered == true)
		{
			image.sprite = LettersManager.Instance.GetTileCoverSprite();
		}
	}
	
}