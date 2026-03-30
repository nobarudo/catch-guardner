using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// 移動スピード（インスペクターから後で調整できます）
	[Export]
	public float Speed { get; set; } = 400.0f;
	
	public override void _Ready()
	{
		// 画面（ビューポート）のサイズを取得します
		Vector2 screenSize = GetViewportRect().Size;

		// X座標：画面の横幅の半分（中央）
		float startX = screenSize.X / 2;
		
		// Y座標：画面の縦幅から少し引いた値（下から50ピクセル上の位置）
		float startY = screenSize.Y - 50.0f; 

		// プレイヤーの現在位置（GlobalPosition）に新しい座標を設定します
		GlobalPosition = new Vector2(startX, startY);
	}

	// 物理演算や移動の処理は _PhysicsProcess に書きます
	public override void _PhysicsProcess(double delta)
	{
		// マウスの左ボタン（またはスマホのタップ）が押されているかチェック
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			// マウス・タップされている画面上の現在位置を取得
			Vector2 targetPosition = GetGlobalMousePosition();
			
			// プレイヤーから目標地点への「向きと距離（ベクトル）」を計算
			Vector2 direction = targetPosition - GlobalPosition;

			// ターゲットに少し近づくまでは移動する（クリック位置でガタガタ震えるのを防ぐため）
			if (direction.Length() > 10.0f)
			{
				// 方向（direction.Normalized）にスピードを掛けて速度を設定
				Velocity = direction.Normalized() * Speed;
				
				// 実際に移動させる（障害物にぶつかると滑る便利なメソッドです）
				MoveAndSlide();
				return; // ここで処理を終えて、下の「停止」処理をスキップします
			}
		}

		// クリックされていない時、または目的地に到着した時は停止する
		Velocity = Vector2.Zero;
	}
}
