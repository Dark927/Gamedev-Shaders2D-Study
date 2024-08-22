using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DissolveControl : MonoBehaviour
{
    [ColorUsage(true, true)][SerializeField] private Color _dissolveColor = Color.white;
    [SerializeField] private float _dissolveTimeInSec = 1f;

    private float _targetDissolveOutAmount = 1f;
    private float _targetDissolveInAmount = -0.1f;
    private bool _isDissolving = false;

    private Material _material;

    public void DissolveOut()
    {
        if (_isDissolving)
        {
            return;
        }

        StartCoroutine(DissolveRoutine(_targetDissolveOutAmount));
    }

    public void DissolveIn()
    {
        if (_isDissolving)
        {
            return;
        }

        StartCoroutine(DissolveRoutine(_targetDissolveInAmount));
    }


    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DissolveIn();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            DissolveOut();
        }
    }

    private IEnumerator DissolveRoutine(float targetDissolveAmount)
    {
        _isDissolving = true;

        _material.SetColor("_OutlineColor", _dissolveColor);
        float startDissolveAmount = _material.GetFloat("_DissolveAmount");

        bool dissolved = Mathf.Approximately(startDissolveAmount, targetDissolveAmount);

        if (!dissolved)
        {
            float dissolveAmount = 0;
            float elapsedTime = 0;

            while (elapsedTime < _dissolveTimeInSec)
            {
                elapsedTime += Time.deltaTime;
                dissolveAmount = Mathf.Lerp(startDissolveAmount, targetDissolveAmount, elapsedTime / _dissolveTimeInSec);
                _material.SetFloat("_DissolveAmount", dissolveAmount);

                yield return null;
            }
            _material.SetFloat("_DissolveAmount", targetDissolveAmount);
        }

        _isDissolving = false;
    }
}
