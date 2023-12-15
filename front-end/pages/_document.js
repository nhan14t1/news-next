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
          <meta name='description' content='Showbiz 365' />
          <meta httpEquiv='Content-Type' content='text/html; charset=utf-8' />
          <meta property="og:type" content="article" />
          <meta property="og:locale" content="vi_VN" />
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
