using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakanishiWeb.Const
{
    public class LanguageTable
    {
        /// <summary>
        /// ページごとの文言をDBから取得するためのページID
        /// </summary>
        public enum PageId
        {
            Common = 0,
            Login,
            Main,
            ClientList,
            PRODUCT_PAGE_ID,
            AlertList,
            Detail,
            History,
            UserEdit,
        }

        public enum CommonPageStrId
        {
            Common_0 = 0,    // 様
            Common_1,    // 最終ログイン
            Common_2,    // ログアウト
            Common_3,    // TOP
            Common_4,    // エンドユーザー一覧
            Common_5,    // サービス管理
            Common_6,    // 機械アラート
            Common_7,    // 部品交換アラート
            Common_8,    // 検索項目
            Common_9,    // 得意先CD
            Common_10,    // 得意先名
            Common_11,    // エンドユーザーCD
            Common_12,    // エンドユーザー名
            Common_13,    // 設置開始
            Common_14,    // 製品群
            Common_15,    // 品名
            Common_16,    // 型式
            Common_17,    // S/N
            Common_18,    // シリアルナンバー
            Common_19,    // 稼働時間
            Common_20,    // 担当支店営業所
            Common_21,    // 顧客呼称
            Common_22,    // 保証契約開始日
            Common_23,    // 郵便番号
            Common_24,    // 住所
            Common_25,    // 全
            Common_26,    // 件中
            Common_27,    // 件まで表示
            Common_28,    // 表示条件
            Common_29,    // 全て
            Common_30,    // 得意先コードを入力
            Common_31,    // エンドユーザーコードを入力
            Common_32,    // シリアルナンバーを入力
            Common_33,    // 〇時間以上
            Common_34,    // 顧客呼称を入力
            Common_35,    // 郵便番号を入力
            Common_36,    // 住所を入力
            Common_37,    // メールアドレスを入力
            Common_38,    // 検索
            Common_39,    // メール
            Common_40,    // 得意先
            Common_41,    // エンドユーザー
            Common_42,    // 現在選択中の製品
            Common_43,    // 年
            Common_44,    // 月
            Common_45,    // 日
            Common_46,    // あり
            Common_47,    // なし
            Common_48,    // 製品一覧
            Common_49,    // メンテナンス管理
            Common_50,    // 終了
            Common_51,    // 回
            Common_52,    // 時間
            Common_53,    // 解除日時
            Common_54,    // データがありません
            Common_55,    // 交換完了
            Common_56,    // 設置日
            Common_57,    // 詳細
            Common_58,    // 閉じる
            Common_59,    // 機械
            Common_60,    // 部品
            Common_61,    // 部品
            Common_62,    // まで
            Common_63,    // CSVダウンロード
            Common_64,    // ユーザ管理
            Common_65,    // 適用
            Common_66,    // データがありません
            Common_67,    // 設定が変更されました
            Common_68,    // TOP画面へ
            Common_69,    // ログアウトしてよろしいですか？
            Common_70,    // 様


        }

        public enum LoginPageStrId
        {
            Login_0 = 0,    // タイトル
            Login_1,    // ログインID
            Login_2,    // パスワード
            Login_3,    // ログイン
            Login_4    // IDまたはパスワードを確認してください
        }

        public enum MainPageStrId
        {
            Main_0 = 0,    // 機器一覧
            Main_1,    // 通電時間
            Main_2,    // 製品年齢(日)
        }

        public enum ClientListPageStrId
        {
            ClientList_0 = 0,    // エンドユーザー一覧
        }

        public enum ProductPageStrId
        {
            Product_0 = 0,    // 製品一覧
        }

        public enum AlertListPageStrId
        {
            AlertList_0 = 0,    // 一覧
            AlertList_1,    // 機械アラート
            AlertList_2,    // 部品交換アラート
            AlertList_3,    // 発生日時
            AlertList_4,    // アラート名
            AlertList_5,    // アラート開始
            AlertList_6,    // アラート終了
            AlertList_7,    // アラートレベル
            AlertList_8,    // 発生日時

        }

        public enum DetailPageStrId
        {
            Detail_0 = 0,    // アラート・メンテナンス時期情報
            Detail_1,    // 更新
            Detail_2,    // アラート履歴を表示
            Detail_3,    // 発生回数
            Detail_4,    // アラート名
            Detail_5,    // 内容
            Detail_6,    // 発生時刻
            Detail_7,    // 推奨交換目安
            Detail_8,    // 稼働量
            Detail_9,    // 推奨交換目安までの残量
            Detail_10,    // 交換日
            Detail_11,    // 納品日
            Detail_12,    // 交換回数
            Detail_13,    // のアラート発生状況
            Detail_14,    // 部品名
            Detail_15,    // 日別データ
            Detail_16,    // 月別データ
            Detail_17,    // 解除時刻
            Detail_18,    // 交換部品情報
            Detail_19,    // 現在
            Detail_20,    // 交換完了
            Detail_21,    // 交換部品情報
            Detail_22,    // 現在
            Detail_23,    // 選択された機器には登録されたパーツがありません
            Detail_24,    // 選択された製品の機械アラート情報はありません
            Detail_25,    // 前の日
            Detail_26,    // 次の日
            Detail_27,    // 残り回数
            Detail_28,    // 残り時間
            Detail_29,    // アラートメール設定
            Detail_30,    // の情報
            Detail_31,    // アラートグラフ
            Detail_32,    // 機械アラートリスト

        }

        public enum HistoryPageStrId
        {
            History_0 = 0,    // アラート詳細履歴
            History_1,    // 前のページへ戻る
            History_2,    // 期間を指定
            History_3,    // ～
            History_4,    // 昇順
            History_5,    // 降順
            History_6,    // 並べ替え
            History_7,    // アラート名
            History_8,    // アラート内容
            History_9,    // 発生日時
            History_10,    // 稼働時間
            History_11,    // アラート詳細
            History_12,    // コール
            History_13,    // アラートレベル
            History_14,    // エラータイプ
            History_15,    // パーツ名
            History_16,    // 緊急
            History_17,    // 警告
            History_18,    // 注意
            History_19,    // 通知
            History_20,    // 終了時刻
            History_21,    // 解除日時
        }

        public enum UserEditPageStrId
        {
            UserEdit_0 = 0,    // ユーザ名
            UserEdit_1,    // ID
            UserEdit_2,    // 現在のパスワード（必須）
            UserEdit_3,    // パスワードの変更（任意）
            UserEdit_4,    // パスワードの変更（確認用）
            UserEdit_5,    // メール受信可能時間帯
            UserEdit_6,    // 数字、英小文字、英大文字それぞれを含む8-16文字
            UserEdit_7,    // 時間帯は開始時刻が終了時刻より先になるように設定してください
            UserEdit_8,    // パスワードが間違っています

        }
    }
}
