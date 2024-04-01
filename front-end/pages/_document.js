import Document, { Html, Head, Main, NextScript } from 'next/document';

class MyDocument extends Document {
  static async getInitialProps(ctx) {
    const initialProps = await Document.getInitialProps(ctx);
    return { ...initialProps };
  }

  render() {
    return (
      <Html lang='vi'>
        <Head>
          <link rel='icon' href='/favicon.svg' />
          <meta httpEquiv='Content-Type' content='text/html; charset=utf-8' />
          <meta property="og:type" content="article" />
          <meta property="og:locale" content="vi_VN" />
          <meta property="fb:app_id" content="24213169571660041" />
          <meta name="google-adsense-account" content="ca-pub-3704994963081548" />
          <script async src="https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-3704994963081548"
            crossOrigin="anonymous" crossorigin="anonymous"></script>
        </Head>
        <body>
          <Main />
          <NextScript />
        </body>
      </Html>
    );
  }
}

export default MyDocument;
