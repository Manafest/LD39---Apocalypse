using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

	public CameraMover cameraMover;
	public PlayerManager player;

	// Use this for initialization
	void Start () {
		
	}
		
	// Update is called once per frame
    void Update () {
		if (PlayerManager.locked) {
			return;
		}
		// If the mouse os over the UI then do nothing.
		if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.collider.gameObject;

            if (objectHit == null) {
                return;
            }
			if (Input.GetMouseButtonDown(1)) {
				Spells.ResetHighlights ();
				Spells.selectedSpell = Spell.NONE;
			}

			if (Spells.selectedSpell != Spell.NONE) {
				switch (Spells.selectedSpell) {
				case Spell.CONE:
					Spells.ConeOfFlame (objectHit.GetComponent<Coordinates> ().xy);
					break;
				case Spell.CHAIN:
					var list = PlayerManager.currentLevel.IUnits.Where (x => x.GetPosition () == objectHit.GetComponent<Coordinates> ().xy);
					if (list.Any ()) {
						Spells.ChainLighting (list.First());
					} else {
						Spells.ResetHighlights ();
					}
					break;
				case Spell.BOMB:
					Spells.InsanityBomb (objectHit.GetComponent<Coordinates> ().xy);
					break;
				case Spell.METEOR:
					Spells.Meteor (objectHit.GetComponent<Coordinates> ().xy);
					break;
				case Spell.POOL:
					Spells.PoolsOfDeath (objectHit.GetComponent<Coordinates> ().xy);
					break;
				default:
					break;
				}

				if (Input.GetMouseButtonDown(0)) {
					Spells.ActivateSpell ();
					Spells.ResetHighlights ();
					Spells.selectedSpell = Spell.NONE;
				}
			}
        }
    }
}
