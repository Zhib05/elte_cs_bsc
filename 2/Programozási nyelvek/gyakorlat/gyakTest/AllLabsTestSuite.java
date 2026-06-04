
import org.junit.platform.suite.api.SelectClasses;
import org.junit.platform.suite.api.Suite;

@Suite
@SelectClasses({
      Lab04TestSuite.class
    , Lab06TestSuite.class
    , Lab07TestSuite.class
//    , Lab08TestSuite.class
//    , Lab09TestSuite.class
//    , Lab10TestSuite.class
    , Lab11TestSuite.class
})
public class AllLabsTestSuite {}
