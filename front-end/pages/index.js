import Head from 'next/head';
import Link from 'next/link';
import {
  Loader,
  Card,
  TinyCard,
  ScrollToTop,
} from '../components';
import styles from '../styles/Home.module.scss';
import useComboFetch from '../components/Hooks/useComboFetch';
import { BASE_THUMBNAIL_URL, CATEGORIES, TEST_IMAGE_URL, WEB_NAME } from '../shared/constants/app-const';

function Home() {
  const { loading, error, vietNamPosts, globalPosts, videoPosts, topPosts } =
    useComboFetch();

  // Combo grid layout
  const sectionTopStory = () => {
    const primary = topPosts.slice(0, 1);
    const secondary = topPosts.slice(-3);
    const tertiary = topPosts.slice(1, 5);

    return (
      <>
        <section className={styles.grid_wrap}>
          <div className={styles.grid}>
            {sectionCards(primary, '#388E3C')}
            {titleOnlyCards(tertiary, [
              '#D32F2F',
              '#FFC107',
              '#2196F3',
              '#388E3C',
            ])}
          </div>
        </section>
        {sectionCards(secondary)}
      </>
    );
  };

  // Only title news card
  const titleOnlyCards = (content, bgColor) => {
    return (
      <section className={styles.grid_wrap}>
        <div className={styles.grid}>
          {content.map((item, idx) => (
            <Link
              key={`lnkTinyNews${item.id}`}
              href={{
                pathname: '/article/',
                query: { id: item.slug },
              }}
            >
              <TinyCard title={item.title} bgColor={bgColor[idx]} />
            </Link>
          ))}
        </div>
      </section>
    );
  };

  // 3-column layout grid
  const sectionCards = (content, bgColor) => {
    if (!content || !content.length) {
      return <div className='pt-5'>Không có nội dung</div>
    }
    return (
      <section className={styles.grid_wrap}>
        <div className={styles.grid}>
          {content.map((item) => (
            <Link
              key={`lnkNews${item.id}`}
              href={{
                pathname: `/article/`,
                query: { id: item.slug },
              }}
            >
              <Card
                title={item.title}
                headline={item.introText}
                thumbnail={item.thumbnailFileName ? `${BASE_THUMBNAIL_URL}/${item.thumbnailFileName}` : TEST_IMAGE_URL}
                bgColor={bgColor}
              />
            </Link>
          ))}
        </div>
      </section>
    );
  };

  const handleSorting = (e) => {
    // setSorting(e.target.value);
  };

  // Error handling and loading
  const getContent = () => {
    if (error)
      return (
        <div className={styles.empty}>
          <div className={styles.icon}></div>
          <h2>Can't display page</h2>
          <h4>We could not find what you were looking for.</h4>
        </div>
      );
    if (loading)
      return (
        <div className='loader-container'>
          <Loader />
        </div>
      );
    return (
      <main className={styles.main}>
        <div className={styles.heading}>
          <h1 className='mt-3'>Đọc nhiều nhất</h1>
          {/* <div className={styles.toolkit}>
            <Button
              onClick={() => {
                router.push('/bookmarks');
              }}
            >
              View Bookmark
            </Button>
            <Select onChange={handleSorting} orderBy={sorting} />
          </div> */}
        </div>

        {sectionTopStory()}

        <h2>{CATEGORIES.VietNam.name}</h2>
        {sectionCards(vietNamPosts, '#d32f2f')}

        <h2>{CATEGORIES.Global.name}</h2>
        {sectionCards(globalPosts, '#FFC107')}

        {/* <h2>Video</h2>
        {sectionCards(videoPosts, '#388E3C')} */}
      </main>
    );
  };

  return (
    <div className='container'>
      <Head>
        <title>{`${WEB_NAME} - Trang chủ`}</title>
      </Head>

      {getContent()}
      <ScrollToTop />
    </div>
  );
}

export default Home;
