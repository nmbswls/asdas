using UnityEngine;
using System.Collections;

public interface FieldClickableObject
{
	void onDrag (Vector3 posInScreenView);

	void startDrag (Vector3 posDeltaInScreenView);

	void onClick (Vector3 posInScreenView);

	void endDrag (Vector3 posInScreenView);
}

